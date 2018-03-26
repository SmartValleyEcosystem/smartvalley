using System.Collections.Generic;
using System.Linq;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Projects.Responses
{
    public class ProjectAboutResponse
    {
        public long ProjectId { get; set; }

        public string Description { get; set; }

        public string Facebook { get; set; }

        public string Linkedin { get; set; }

        public string BitcoinTalk { get; set; }

        public string Medium { get; set; }

        public string Reddit { get; set; }

        public string Telegram { get; set; }

        public string Twitter { get; set; }

        public string Github { get; set; }

        public List<ProjectTeamMemberResponse> TeamMembers { get; set; }

        public static ProjectAboutResponse Create(Project project, IReadOnlyCollection<ProjectTeamMember> teamMembers)
        {
            return new ProjectAboutResponse
                   {
                       ProjectId = project.Id,
                       Description = project.Description,
                       Facebook = project.SocialNetworks.Facebook,
                       Linkedin = project.SocialNetworks.Linkedin,
                       BitcoinTalk = project.SocialNetworks.BitcoinTalk,
                       Medium = project.SocialNetworks.Medium,
                       Reddit = project.SocialNetworks.Reddit,
                       Telegram = project.SocialNetworks.Telegram,
                       Twitter = project.SocialNetworks.Twitter,
                       Github = project.SocialNetworks.Github,
                       TeamMembers = teamMembers.Select(ProjectTeamMemberResponse.Create).ToList()
                   };
        }
    }
}