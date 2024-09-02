using GrowAcc.Models;

namespace GrowAcc.Database
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) 
        {
            _context = context;
        }
        public UserAccount Create(UserAccount account)
        {
            _context.UserAccounts.Add(account);
            return account;
        }

        public void Delete(UserAccount account)
        {
            _context.UserAccounts.Remove(account);
        }

        public UserAccount Get(Guid id) => _context.UserAccounts.Where(x => x.Id == id).FirstOrDefault();

        public void Update(UserAccount account)
        {
            _context.UserAccounts.Update(account);
        }
    }
}
