using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace SmartValley.WebApi.Projects.Requests
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ProjectTeamMemberRequest
    {
        public string FullName { get; set; }

        public string Role { get; set; }

        public string About { get; set; }

        public string FacebookLink { get; set; }

        public string LinkedInLink { get; set; }

        public IReadOnlyCollection<SocialMediaRequest> SocialMedias { get; set; }
    }
}