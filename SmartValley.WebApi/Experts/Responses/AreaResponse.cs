using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Experts.Responses
{
    public class AreaResponse
    {
        public AreaType Id { get; set; }

        public string Name { get; set; }

        public static AreaResponse Create(Area area)
        {
            return new AreaResponse
                   {
                       Id = (AreaType) area.Id,
                       Name = area.Name
                   };
        }
    }
}
