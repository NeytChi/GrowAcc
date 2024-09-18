using GrowAcc.Models;

namespace GrowAcc.Responses
{
    public class UserAccountResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }

        public UserAccountResponse(UserAccount userAccount)
        {
            Id = userAccount.Id;
            FirstName = userAccount.FirstName;
            LastName = userAccount.LastName;
            Email = userAccount.Email;
            PhoneNumber = userAccount.PhoneNumber;
            CreatedAt = userAccount.CreatedAt;
            LastUpdatedAt = userAccount.LastUpdatedAt;
        }
    }
}
