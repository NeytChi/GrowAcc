namespace GrowAcc.Core
{
    public class Success
    {
        public bool success { get; set; }
        public string message { get; set; }
        public Success(bool success, string message)
        {
            this.success = success;
            this.message = message;
        }
    }
}
