namespace SmartValley.WebApi.Users.Responses
{
    public class GetUserResponse
    {
        public string Address { get; set; }

        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }
    }
}
