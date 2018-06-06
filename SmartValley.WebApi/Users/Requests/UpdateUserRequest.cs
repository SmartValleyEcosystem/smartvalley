namespace SmartValley.WebApi.Users.Requests
{
    public class UpdateUserRequest
    {
        public string FirstName { get; set; }
        
        public string SecondName { get; set; }

        public string Bitcointalk { get; set; }

        public string About { get; set; }
    }
}