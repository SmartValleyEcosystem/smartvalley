using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.Users
{
    public class UpdateUserRequest
    {
        public string FirstName { get; set; }
        
        public string SecondName { get; set; }
    }
}