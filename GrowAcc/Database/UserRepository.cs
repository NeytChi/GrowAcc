using GrowAcc.Models;
using Microsoft.EntityFrameworkCore;

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
            _context.UserAccounts.AddAsync(account);
            _context.SaveChangesAsync();
            return account;
        }

        public void Delete(UserAccount account)
        {
            _context.UserAccounts.Remove(account);
            _context.SaveChanges();
        }

        public Task<UserAccount> Get(Guid id) => _context.UserAccounts.Where(x => x.Id == id).SingleOrDefaultAsync();
        public Task<UserAccount> Get(string email) => _context.UserAccounts.Where(x => x.Email == email).SingleOrDefaultAsync();
        public Task<UserAccount> GetByConfirmToken(string confirmToken) => _context.UserAccounts.Where(x => x.ConfirmToken == confirmToken).SingleOrDefaultAsync();

        public void Update(UserAccount account)
        {
            _context.UserAccounts.Update(account);
            _context.SaveChanges();
        }
    }
}
