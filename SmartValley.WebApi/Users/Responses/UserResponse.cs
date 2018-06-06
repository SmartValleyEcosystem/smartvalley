using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Users.Responses
{
    public class UserResponse
    {
        public string Address { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string About { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public string Bitcointalk { get; set; }

        public static UserResponse Create(User user)
        {
            return new UserResponse
            {
                Address = user?.Address,
                Email = user?.Email,
                FirstName = user?.FirstName,
                LastName = user?.SecondName,
                Bitcointalk = user?.BitcointalkLink,
                About = user?.About,
                IsEmailConfirmed = user != null && user.IsEmailConfirmed
            };
        }
    }
}