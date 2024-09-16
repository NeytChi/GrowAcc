using GrowAcc.Models;

namespace GrowAcc.Database
{
    public interface IUserRepository
    {
        public UserAccount Create(UserAccount account);
        public void Update(UserAccount account);
        public void Delete(UserAccount account);
        public UserAccount Get(Guid id);
        public UserAccount Get(string email);
    }
}
