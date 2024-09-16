namespace GrowAcc.BusinessFlow.Smtp
{
    public class SmtpSettings
    {
        public string fromAddress { get; set; }
        public string password { get; set; }
        public string server { get; set; }
        public int port { get; set; }
        public bool ssl { get; set; }
        public bool enableSending { get; set; }
    }
}
