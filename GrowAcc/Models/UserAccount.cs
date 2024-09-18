using GrowAcc.Requests;

namespace GrowAcc.Models
{
    public class UserAccount
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string PhoneNumber { get; set; }
        public string ConfirmToken { get; set; }
        public bool AccountConfirmed { get; set; }
        public DateTimeOffset ConfirmedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public bool Deleted { get; set; }
        public DateTimeOffset DeletedAt { get; set; }
        public UserAccount()
        {

        }
        public UserAccount(UserAccountRegistrationRequest request, string hashedPassword, string storedSalt, string confirmToken)
        {
            Email = request.Email;
            FirstName = request.FirstName;
            LastName = request.LastName;
            PhoneNumber = request.PhoneNumber;
            PasswordHash = hashedPassword;
            PasswordSalt = storedSalt;
            CreatedAt = DateTimeOffset.UtcNow;
            AccountConfirmed = false;
            ConfirmToken = confirmToken;
        }
    }
}
