namespace GrowAcc.Core
{
    public class SuccessData
    {
        public bool success { get; set; }
        public object data { get; set; }
        public SuccessData(bool success, object data) 
        {
            this.success = success;
            this.data = data;
        }
    }
}
