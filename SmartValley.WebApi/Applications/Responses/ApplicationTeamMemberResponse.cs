using SmartValley.Domain.Entities;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace SmartValley.WebApi.Applications.Responses
{
    public class ApplicationTeamMemberResponse
    {
        public string FullName { get; set; }

        public string Role { get; set; }

        public string About { get; set; }

        public string PhotoName { get; set; }

        public static ApplicationTeamMemberResponse Create(ApplicationTeamMember teamMember)
        {
            return new ApplicationTeamMemberResponse
                   {
                       FullName = teamMember.FullName,
                       About = teamMember.About,
                       PhotoName = teamMember.PhotoUrl,
                       Role = teamMember.Role
                   };
        }
    }
}