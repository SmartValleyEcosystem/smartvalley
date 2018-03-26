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

        public string LinkenIn { get; set; }

        public static ProjectTeamMemberResponse Create(ProjectTeamMember teamMember)
        {
            return new ProjectTeamMemberResponse
                   {
                       Id = teamMember.Id,
                       FullName = teamMember.FullName,
                       About = teamMember.About,
                       PhotoUrl = teamMember.PhotoUrl,
                       Role = teamMember.Role,
                       Facebook = teamMember.SocialNetworks.Facebook,
                       LinkenIn = teamMember.SocialNetworks.Linkedin
                   };
        }
    }
}