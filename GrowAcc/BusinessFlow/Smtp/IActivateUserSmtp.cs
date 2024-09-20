namespace GrowAcc.BusinessFlow.Smtp
{
    public interface IActivateUserSmtp
    {
        void ConfirmUserAccount(string email, string token, string culture);
        void ChangePassword(string email, string newPassword, string culture);
    }
}
