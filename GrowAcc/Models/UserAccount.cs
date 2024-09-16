using GrowAcc.Requests;

namespace GrowAcc.Models
{
    public class UserAccount
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public bool AccountConfirmed { get; set; }
        public DateTime ConfirmedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public bool Deleted { get; set; }
        public DateTime DeletedAt { get; set; }
        public UserAccount()
        {

        }
        public UserAccount(UserAccountRegistrationRequest request)
        {
            Email = request.Email;
            FirstName = request.FirstName;
            LastName = request.LastName;
            PhoneNumber = request.PhoneNumber;
            Password = request.Password;
            CreatedAt = DateTime.UtcNow;
            AccountConfirmed = false;
        }
    }
}
