using System.Net.Mail;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Options;

namespace GrowAcc.BusinessFlow.Smtp
{
    public class SmtpSender : ISmtpSender
    {
        private readonly ILogger _logger;
        public bool emailEnable = true;
        private string gmailServer = "smtp.gmail.com";
        private int gmailPort = 587;
        private string domen = "development";
        private string mailAddress;
        private string mailPassword;
        private MailAddress from;
        private SmtpClient smtp;

        public SmtpSender(ILogger<SmtpSender> logger, IOptions<SmtpSettings> options)
        {
            _logger = logger;
            OnConfigure(options.Value);
        }
        public void OnConfigure(SmtpSettings config)
        {
            mailAddress = config.fromAddress;
            mailPassword = config.password;
            gmailServer = config.server;
            gmailPort = config.port;
            emailEnable = config.enableSending;
            if (mailAddress != null)
            {
                smtp = new SmtpClient(gmailServer, gmailPort);
                smtp.Credentials = new NetworkCredential(mailAddress, mailPassword);
                from = new MailAddress(mailAddress, domen);
                smtp.EnableSsl = config.ssl;
            }
            ServicePointManager.ServerCertificateValidationCallback =
            delegate (
                object s,
                X509Certificate certificate,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors
            )
            {
                return true;
            };
        }
        public async void Send(string email, string subject, string text)
        {
            var to = new MailAddress(email);
            var message = new MailMessage(from, to)
            {
                Subject = subject,
                Body = text,
                IsBodyHtml = true
            };
            try
            {
                _logger.LogInformation("Sending message to " + email);
                if (emailEnable)
                    smtp.Send(message);
                _logger.LogInformation("Message sent to " + email);
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to send email to " + email);
                _logger.LogError("Exception message: " + e.Message);
                _logger.LogError("Inner exception: " + e.InnerException?.Message ?? "");
            }
        }


    }
}