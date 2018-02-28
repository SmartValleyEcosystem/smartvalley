using System.Collections.Generic;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Projects.Responses
{
    public class AreaExpertResponse
    {
        public Area Area { get; set; }

        public IEnumerable<string> Addresses { get; set; }
    }
}
