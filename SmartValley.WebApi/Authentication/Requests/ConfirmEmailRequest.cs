namespace SmartValley.WebApi.Authentication.Requests
{
    public class ConfirmEmailRequest
    {
        public string Address { get; set; }

        public string Token { get; set; }
    }
}