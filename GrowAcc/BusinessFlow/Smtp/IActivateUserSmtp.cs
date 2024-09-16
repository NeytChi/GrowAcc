namespace GrowAcc.BusinessFlow.Smtp
{
    public interface IActivateUserSmtp
    {
        void Send(string email, string token, string culture);
    }
}
