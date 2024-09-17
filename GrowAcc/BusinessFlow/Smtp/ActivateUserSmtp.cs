namespace GrowAcc.BusinessFlow.Smtp
{
    public class ActivateUserSmtp : IActivateUserSmtp
    {
        private ISmtpSender _sender;

        public ActivateUserSmtp(ISmtpSender sender)
        {
            _sender = sender;
        }
        public void Send(string email, string token, string culture)
        {
            _sender.Send(email, "Activation link", token);
        }
    }
}
