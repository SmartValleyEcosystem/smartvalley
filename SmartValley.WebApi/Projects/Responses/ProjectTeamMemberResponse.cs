using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Projects.Responses
{
    public class ProjectTeamMemberResponse
    {
        public string FullName { get; set; }

        public string Role { get; set; }

        public string About { get; set; }

        public string PhotoName { get; set; }

        public static ProjectTeamMemberResponse Create(ProjectTeamMember teamMember)
        {
            return new ProjectTeamMemberResponse
            {
                       FullName = teamMember.FullName,
                       About = teamMember.About,
                       PhotoName = teamMember.PhotoUrl,
                       Role = teamMember.Role
                   };
        }
    }
}
