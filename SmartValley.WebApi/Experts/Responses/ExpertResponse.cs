using System.Collections.Generic;
using System.Linq;
using SmartValley.Domain;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Experts.Responses
{
    public class ExpertResponse
    {
        public string Address { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string About { get; set; }

        public bool IsAvailable { get; set; }

        public bool IsInHouse { get; set; }

        public IReadOnlyCollection<AreaResponse> Areas { get; set; }

        public static ExpertResponse Create(Expert expert)
        {
            return new ExpertResponse
                   {
                       Address = expert.User.Address,
                       Email = expert.User.Email,
                       About = expert.About,
                       IsAvailable = expert.IsAvailable,
                       IsInHouse = expert.IsInHouse,
                       FirstName = expert.User.FirstName,
                       SecondName = expert.User.SecondName,
                       Areas = expert.ExpertAreas.Select(j => new AreaResponse {Id = j.Area.Id.FromDomain(), Name = j.Area.Name}).ToArray()
                   };
        }
    }
}
