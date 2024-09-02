using GrowAcc.Models;

namespace GrowAcc.Database
{
    public interface IUserRepository
    {
        public UserAccount Create(UserAccount account);
        public void Update(UserAccount account);
        public void Delete(Guid id);
        public UserAccount Get(Guid id);
    }
}
