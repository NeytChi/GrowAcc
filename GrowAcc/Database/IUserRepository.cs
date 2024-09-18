using GrowAcc.Models;

namespace GrowAcc.Database
{
    public interface IUserRepository
    {
        public UserAccount Create(UserAccount account);
        public void Update(UserAccount account);
        public void Delete(UserAccount account);
        public Task<UserAccount> Get(Guid id);
        public Task<UserAccount> GetByConfirmToken(string confirmToken);
        public Task<UserAccount> Get(string email);
    }
}
