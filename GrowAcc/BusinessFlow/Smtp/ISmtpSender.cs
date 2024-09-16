namespace GrowAcc.BusinessFlow.Smtp
{
    public interface ISmtpSender
    {
        void Send(string email, string subject, string text);
    }
}
