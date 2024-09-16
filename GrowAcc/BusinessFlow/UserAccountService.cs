using GrowAcc.Models;
using GrowAcc.Database;
using LanguageExt.Common;
using GrowAcc.Requests;
using GrowAcc.BusinessFlow.Smtp;
using LanguageExt.Pipes;
using LanguageExt.Pretty;

namespace GrowAcc.BusinessFlow
{
    public interface IUserAccountService
    {
        Task<Result<UserAccount>> SingUp(UserAccountRegistrationRequest request);
    }
    public class UserAccountService : IUserAccountService
    {
        private IUserRepository _repository { get; set; }
        private IActivateUserSmtp _activateUser {  get; set; }
        private ILogger _logger { get; set; }

        public UserAccountService(IUserRepository repository, IActivateUserSmtp activateUserSmtp, ILogger logger)
        {
            _repository = repository;
            _activateUser = activateUserSmtp;
            _logger = logger;
        }

        public async Task<Result<UserAccount>> SingUp(UserAccountRegistrationRequest request)
        {
            var currentUser = _repository.Get(request.Email);
            if (currentUser == null)
            {
                currentUser = new UserAccount(request);
                currentUser = _repository.Create(currentUser);
                // Дописати код для різних культур, котрі використовуются користувачем. Щоб листи котрі він отримувати відповідали його мові.
                // Дописати код, який саме механізм підтвердження буде використовуватися. Це може бути рандомно-згенерована строка або Identity структура. 
                _activateUser.Send(currentUser.Email, "", "");
                _logger.LogInformation($"User with email {currentUser.Email} has been successfully registered.");
                return currentUser;
            }
            else if (currentUser.Deleted)
            {
                currentUser.Deleted = false;
                _repository.Update(currentUser);
                _logger.LogInformation($"User with email {currentUser.Email} has been successfully restored.");
                return currentUser;
            }
            _logger.LogWarning($"User with email {request.Email} was not found.");
            return new Result<UserAccount>();
        }
    }
}
