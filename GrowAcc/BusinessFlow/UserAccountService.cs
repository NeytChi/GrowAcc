using GrowAcc.Models;
using GrowAcc.Database;
using GrowAcc.Requests;
using GrowAcc.BusinessFlow.Smtp;
using GrowAcc.Core;
using CSharpFunctionalExtensions;
using GrowAcc.Culture;
using GrowAcc.Responses;

namespace GrowAcc.BusinessFlow
{
    public interface IUserAccountService
    {
        Task<IResult<UserAccountResponse, DomainError>> Registration(UserAccountRegistrationRequest request, string culture = "eng");
        Task<IResult<UserAccountResponse, DomainError>> ConfirmEmailByToken(string token, string culture);
    }
    public class UserAccountService : IUserAccountService
    {
        private readonly IHttpContextAccessor _http;
        private IUserRepository _repository;
        private IActivateUserSmtp _activateUser;
        private ILogger _logger;
        private UserValidator _validator = new UserValidator();

        public UserAccountService(IUserRepository repository, 
            IActivateUserSmtp activateUserSmtp, 
            ILogger<UserAccountService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _activateUser = activateUserSmtp;
            _logger = logger;
            _http = httpContextAccessor;
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
                _activateUser.Send(newUser.Email, 
                    $"{_http.HttpContext.Request.Scheme}://{_http.HttpContext.Request.Host}/User/confirm?token={confirmToken}", culture);
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

        public async Task<Result<UserAccount>> Login(UserAccountLoginRequest request)
        {

            return new Result<UserAccount>();
        }
        public async Task<IResult<UserAccountResponse, DomainError>> ConfirmEmailByToken(string token, string culture)
        {
            var user = await _repository.GetByConfirmToken(token); 
            if (user == null)
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
    }
}
