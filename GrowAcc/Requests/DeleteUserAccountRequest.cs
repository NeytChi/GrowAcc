namespace GrowAcc.Requests
{
    public class DeleteUserAccountRequest
    {
        public Guid Id { get; set; }
        public string Password { get; set; }
    }
}
