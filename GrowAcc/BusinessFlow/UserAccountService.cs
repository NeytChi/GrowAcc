using GrowAcc.Models;
using GrowAcc.Database;
using GrowAcc.Requests;
using GrowAcc.BusinessFlow.Smtp;
using GrowAcc.Core;
using CSharpFunctionalExtensions;
using GrowAcc.Culture;
using GrowAcc.Responses;
using Azure.Core;

namespace GrowAcc.BusinessFlow
{
    public interface IUserAccountService
    {
        Task<IResult<UserAccountResponse, DomainError>> Registration(UserAccountRegistrationRequest request, string culture = "eng");
        Task<IResult<UserAccountResponse, DomainError>> ConfirmEmailByToken(string token, string culture);
        Task<IResult<UserAccountResponse, DomainError>> Login(UserAccountLoginRequest request, string culture);
        Task<IResult<bool, DomainError>> ResendConfirmationEmail(string email, string culture);
        Task<IResult<bool, DomainError>> ChangePassword(string email, string culture);
        Task<IResult<bool, DomainError>> DeleteAccount(DeleteUserAccountRequest request, string culture);
        Task<IResult<bool, DomainError>> LogOut();
        Task<IResult<UserAccountResponse, DomainError>> SignUpByGoogle();
        Task<IResult<UserAccountResponse, DomainError>> SingInByGoogle();
    }
    public class UserAccountService : IUserAccountService
    {
        private IUserRepository _repository;
        private IActivateUserSmtp _activateUser;
        private ILogger _logger;
        private UserValidator _validator = new UserValidator();

        public UserAccountService(IUserRepository repository, 
            IActivateUserSmtp activateUserSmtp, 
            ILogger<UserAccountService> logger)
        {
            _repository = repository;
            _activateUser = activateUserSmtp;
            _logger = logger;
            
        }

        public async Task<IResult<UserAccountResponse, DomainError>> Registration(UserAccountRegistrationRequest request, string culture = "eng")
        {
            var errors = new Dictionary<string, string>();
            if (!_validator.IsOkay(request.Email, out errors, culture) ||
                !_validator.IsPasswordTrue(request.Password, request.ConfirmPassword, culture, ref errors))
            {
                _logger.LogWarning("Registration request for the user didn't pass validation.");
                return Result.Failure<UserAccountResponse, DomainValidationError>(DomainValidationError.NotValid(errors));
            }
            var currentUser = await _repository.Get(request.Email);
            if (currentUser == null)
            {
                var passwordKeys = _validator.GetCombinationHashPassword(request.Password);
                var confirmToken = _validator.CreateConfirmToken();
                var newUser = new UserAccount(request, passwordKeys["Password"], passwordKeys["Salt"], confirmToken);
                newUser = _repository.Create(newUser);
                _activateUser.ConfirmUserAccount(newUser.Email, confirmToken, culture);
                _logger.LogInformation($"User with email {newUser.Email} has been successfully registered.");
                return Result.Success<UserAccountResponse, DomainError>(new UserAccountResponse(newUser));
            }
            if (currentUser.Deleted)
            {
                currentUser.Deleted = false;
                _repository.Update(currentUser);
                _logger.LogInformation($"User with email {currentUser.Email} has been successfully restored.");
                return Result.Success<UserAccountResponse, DomainError>(new UserAccountResponse(currentUser));
            }
            _logger.LogWarning($"User is trying to register an already registered account.");
            return Result.Failure<UserAccountResponse, DomainError>(
                DomainError.Conflict(string.Format(CultureConfiguration.Get("UserTryToRegisterActiveAccount", culture), request.Email)));
        }
        public async Task<IResult<UserAccountResponse, DomainError>> ConfirmEmailByToken(string token, string culture)
        {
            var user = await _repository.GetByConfirmToken(token); 
            if (user == null || user.Deleted)
            {
                _logger.LogWarning($"User account wasn't found by this {token} token.");
                return Result.Failure<UserAccountResponse, DomainError>(
                    DomainError.NotFound(CultureConfiguration.Get("UserNotFoundByToken", culture)));
            }
            if (user.AccountConfirmed) 
            {
                _logger.LogWarning($"The user {user.Email} attempted to activate their account using a token, but the account is already active.");
                return Result.Failure<UserAccountResponse, DomainError>(
                    DomainError.Conflict(CultureConfiguration.Get("UserAlreadyConfirmed", culture)));
            }
            user.AccountConfirmed = true;
            user.ConfirmedAt = DateTime.UtcNow;
            user.ConfirmToken = null;
            _repository.Update(user);
            _logger.LogInformation($"The user with email {user.Email} has been successfully confirmed.");
            return Result.Success<UserAccountResponse, DomainError>(new UserAccountResponse(user));
        }
        public async Task<IResult<UserAccountResponse, DomainError>> Login(UserAccountLoginRequest request, string culture)
        {
            var user = await _repository.Get(request.Email);

            if (user == null || user.Deleted)
            {
                _logger.LogWarning($"The user with email {request.Email} attepted to log in, but he wasn't found.");
                return Result.Failure<UserAccountResponse, DomainError>(
                    DomainError.NotFound(string.Format(CultureConfiguration.Get("UserAccountNotFound", culture), request.Email)));
            }
            if (!user.AccountConfirmed)
            {
                _logger.LogWarning($"The user with email {user.Email} attempted to log in, but their account is not activated.");
                return Result.Failure<UserAccountResponse, DomainError>(
                    DomainError.Conflict(string.Format(CultureConfiguration.Get("UserTryToLoginNonActiveAccount", culture), request.Email)));
            }
            if (!_validator.CheckPassword(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                _logger.LogWarning($"The user with email '{user.Email}' attempted to log in, but the password is incorrect.");
                return Result.Failure<UserAccountResponse, DomainError>(
                    DomainError.Conflict(CultureConfiguration.Get("PasswordIncorrect", culture)));
            }
            _logger.LogInformation($"The user with email {user.Email} has logged in successfully.");
            return Result.Success<UserAccountResponse, DomainError>(new UserAccountResponse(user));
        }
        public async Task<IResult<bool, DomainError>> ResendConfirmationEmail(string email, string culture)
        {
            var user = await _repository.Get(email);

            if (user == null || user.Deleted)
            {
                _logger.LogWarning($"The user with email {email} attempted to resend confirmation email, but he wasn't found.");
                return Result.Failure<bool, DomainError>(
                    DomainError.NotFound(string.Format(CultureConfiguration.Get("UserAccountNotFound", culture), email)));
            }
            if (user.AccountConfirmed)
            {
                _logger.LogWarning($"The user with email '{email}' attempted to resend the confirmation email, but their account is already activated.");
                return Result.Failure<bool, DomainError>(
                    DomainError.Conflict(string.Format(CultureConfiguration.Get("UserTryResendConfirmEmail", culture), email)));
            }

            _activateUser.ConfirmUserAccount(email, user.ConfirmToken, culture);
            _logger.LogInformation($"The confirmation email was successfully resent to the user with email {email}.");
            return Result.Success<bool, DomainError>(true);
        }

        public async Task<IResult<bool, DomainError>> ChangePassword(string email, string culture)
        {
            var user = await _repository.Get(email);

            if (user == null || user.Deleted || !user.AccountConfirmed)
            {
                _logger.LogWarning($"The user with email {email} attempted to change password, but he wasn't found.");
                return Result.Failure<bool, DomainError>(
                    DomainError.NotFound(string.Format(CultureConfiguration.Get("UserAccountNotFound", culture), email)));
            }
            var newPassword = _validator.CreateNewPassword();
            var passwordKeys = _validator.GetCombinationHashPassword(newPassword);
            user.PasswordHash = passwordKeys["Password"];
            user.PasswordSalt = passwordKeys["Salt"];
            _repository.Update(user);
            _activateUser.ChangePassword(email, newPassword, culture);
            _logger.LogInformation($"The password for the user with email address {email} has been changed.");
            return Result.Success<bool, DomainError>(true);
        }
        public Task<IResult<UserAccountResponse, DomainError>> SignUpByGoogle()
        {
            throw new NotImplementedException();
        }

        public Task<IResult<UserAccountResponse, DomainError>> SingInByGoogle()
        {
            throw new NotImplementedException();
        }
    }
}
