using GrowAcc.Culture;

namespace GrowAcc.BusinessFlow.Smtp
{
    public class ActivateUserSmtp : IActivateUserSmtp
    {
        private readonly IHttpContextAccessor _http;
        private ISmtpSender _sender;

        public ActivateUserSmtp(ISmtpSender sender, IHttpContextAccessor httpContextAccessor)
        {
            _sender = sender;
            _http = httpContextAccessor;
        }
        public void ConfirmUserAccount(string email, string token, string culture)
        {
            _sender.Send(email,
                CultureConfiguration.Get("ConfirmUserAccountSubject", culture),
                string.Format(CultureConfiguration.Get("ConfirmUserAccountEmail", culture),
                _http.HttpContext.Request.Scheme, 
                _http.HttpContext.Request.Host, token));
        }
        public void ChangePassword(string email, string newPassword, string culture)
        {
            _sender.Send(email, 
                CultureConfiguration.Get("ChangePasswordSubject", culture), 
                string.Format(CultureConfiguration.Get("ChangePasswordEmail", culture), newPassword));
        }
    }
}
