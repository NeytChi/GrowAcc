using GrowAcc.Models;
using GrowAcc.Database;
using LanguageExt.Common;
using GrowAcc.Requests;
using GrowAcc.BusinessFlow.Smtp;

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

        public UserAccountService(IUserRepository repository, IActivateUserSmtp activateUserSmtp)
        {
            _repository = repository;
            _activateUser = activateUserSmtp;
        }

        public async Task<Result<UserAccount>> SingUp(UserAccountRegistrationRequest request)
        {
            var currentUser = _repository.Get(request.Email);
            if (currentUser == null)
            {
                currentUser = new UserAccount(request);
                currentUser = _repository.Create(currentUser);
                // Дописати код для різних культур, котрі використовуются користувачем. Щоб листи котрі він отримувати відповідали його мові.
                // Дописати код, який саме механізм підтвердження буде використовуватися. Це може бути рандомна строка або Identity структура. 
                _activateUser.Send(currentUser.Email, "", "");
                // Send email to confirm 
                // log.Information("Registrate new user, id -> " + user.userId);
                return currentUser;
            }
            else if (currentUser.Deleted)
            {
                currentUser.Deleted = false;
                _repository.Update(currentUser);
                return currentUser;
                // log.Information("User account was restored, id -> " + user.userId);
            }
            return new Result<UserAccount>();
        }
    }
}
