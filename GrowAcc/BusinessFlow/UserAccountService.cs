using GrowAcc.Models;
using GrowAcc.Database;
using GrowAcc.Requests;
using GrowAcc.BusinessFlow.Smtp;
using GrowAcc.Core;
using CSharpFunctionalExtensions;

namespace GrowAcc.BusinessFlow
{
    public interface IUserAccountService
    {
        Task<IResult<UserAccount, DomainError>> Registration(UserAccountRegistrationRequest request);
    }
    public class UserAccountService : IUserAccountService
    {
        private IUserRepository _repository { get; set; }
        private IActivateUserSmtp _activateUser {  get; set; }
        private ILogger _logger { get; set; }
        private UserValidator _validator { get; set; }

        public UserAccountService(IUserRepository repository, IActivateUserSmtp activateUserSmtp, ILogger<UserAccountService> logger)
        {
            _repository = repository;
            _activateUser = activateUserSmtp;
            _logger = logger;
            _validator = new UserValidator();
        }

        public async Task<IResult<UserAccount, DomainError>> Registration(UserAccountRegistrationRequest request)
        {
            var errors = new Dictionary<string, string>();
            if (!_validator.IsOkay(request.Email, out errors) ||
                !_validator.IsPasswordTrue(request.Password, request.ConfirmPassword, ref errors))
            {
                _logger.LogWarning("Registration request for the user didn't pass validation.");
                return Result.Failure<UserAccount, DomainValidationError>(DomainValidationError.NotValid(errors));
            }
            var currentUser = _repository.Get(request.Email);
            if (currentUser == null)
            {
                request.Password = _validator.ConvertPasswordForStore(request.Password);
                currentUser = new UserAccount(request);
                currentUser = _repository.Create(currentUser);
                // Дописати код для різних культур, котрі використовуются користувачем. Щоб листи котрі він отримувати відповідали його мові.
                // Дописати код, який саме механізм підтвердження буде використовуватися. Це може бути рандомно-згенерована строка або Identity структура. 
                _activateUser.Send(currentUser.Email, "", "");
                _logger.LogInformation($"User with email {currentUser.Email} has been successfully registered.");
                return Result.Success<UserAccount, DomainError>(currentUser);
            }
            if (currentUser.Deleted)
            {
                currentUser.Deleted = false;
                _repository.Update(currentUser);
                _logger.LogInformation($"User with email {currentUser.Email} has been successfully restored.");
                return Result.Success<UserAccount, DomainError>(currentUser);
            }
            _logger.LogWarning($"User with email {request.Email} was not found.");
            return Result.Failure<UserAccount, DomainError>(DomainError.NotFound($"User with email {request.Email} was not found."));
        }

        public async Task<Result<UserAccount>> Login(UserAccountLoginRequest request)
        {

            return new Result<UserAccount>();
        }
    }
}
