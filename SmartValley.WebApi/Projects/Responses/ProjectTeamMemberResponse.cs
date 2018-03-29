using SmartValley.Domain;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Projects.Responses
{
    public class ProjectTeamMemberResponse
    {
        public long Id { get; set; }

        public string FullName { get; set; }

        public string Role { get; set; }

        public string About { get; set; }

        public string PhotoUrl { get; set; }

        public string Facebook { get; set; }

        public string LinkedIn { get; set; }

        public static ProjectTeamMemberResponse Create(ProjectTeamMember teamMember)
        {
            return new ProjectTeamMemberResponse
                   {
                       Id = teamMember.Id,
                       FullName = teamMember.FullName,
                       About = teamMember.About,
                       PhotoUrl = teamMember.PhotoUrl,
                       Role = teamMember.Role,
                       Facebook = teamMember.Facebook,
                       LinkedIn = teamMember.Linkedin
                   };
        }

        public static ProjectTeamMemberResponse Create(ScoringApplicationTeamMember teamMember)
        {
            return new ProjectTeamMemberResponse
                   {
                       FullName = teamMember.FullName,
                       About = teamMember.About,
                       Role = teamMember.ProjectRole,
                       Facebook = teamMember.FacebookLink,
                       LinkedIn = teamMember.LinkedInLink
                   };
        }
    }
}