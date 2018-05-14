using System;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Admin.Response
{
    public class UserResponse
    {
        public DateTime? RegistrationDate { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public bool CanCreatePrivateProjects { get; set; }
        
        public static UserResponse Create(User user)
        {
            return new UserResponse
            {
                       Address = user.Address,
                       Email = user.Email,
                       FirstName = user.FirstName,
                       LastName = user.SecondName,
                       CanCreatePrivateProjects = user.CanCreatePrivateProjects,
                       RegistrationDate = user.RegistrationDate
                   };
        }
    }
}
