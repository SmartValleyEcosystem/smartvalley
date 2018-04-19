using System.Collections.Generic;
using System.Linq;
using SmartValley.Domain;

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

        public IReadOnlyCollection<AreaResponse> Areas { get; set; }

        public static ExpertResponse Create(ExpertDetails expertDetails)
        {
            return new ExpertResponse
                   {
                       Address = expertDetails.Address,
                       Email = expertDetails.Email,
                       About = expertDetails.About,
                       IsAvailable = expertDetails.IsAvailable,
                       FirstName = expertDetails.FirstName,
                       SecondName = expertDetails.SecondName,
                Areas = expertDetails.Areas.Select(j => new AreaResponse { Id = j.Id.FromDomain(), Name = j.Name }).ToArray()
                   };
        }
    }
}
