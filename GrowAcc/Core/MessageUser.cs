using GrowAcc.Responses;

namespace GrowAcc.Core
{
    public class MessageUser
    {
        public string message { get; set; }
        public UserAccountResponse user { get; set; }
        public MessageUser(string message, UserAccountResponse user)
        {
            this.message = message;
            this.user = user;
        }
    }
}
