using System.ComponentModel.DataAnnotations;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Projects.Requests
{
    public class SocialMediaRequest
    {
        public SocialMediaType NetworkId { get; set; }

        [Url]
        public string Link { get; set; }
    }
}