using Microsoft.AspNetCore.Http;

namespace SmartValley.WebApi.Projects.Requests
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TeamMemberRequest
    {
        public string FullName { get; set; }

        public string Role { get; set; }

        public string About { get; set; }

        public IFormFile Photo { get; set; }

        public string FacebookLink { get; set; }

        public string LinkedInLink { get; set; }
    }
}