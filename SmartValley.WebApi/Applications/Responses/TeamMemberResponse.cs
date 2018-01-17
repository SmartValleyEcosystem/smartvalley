using SmartValley.Domain.Entities;
using SmartValley.WebApi.TeamMembers;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace SmartValley.WebApi.Applications.Responses
{
    public class TeamMemberResponse
    {
        public Type MemberType { get; set; }

        public string FullName { get; set; }

        public string FacebookLink { get; set; }

        public string LinkedInLink { get; set; }

        public static TeamMemberResponse Create(TeamMember teamMember)
        {
            return new TeamMemberResponse
                   {
                       FacebookLink = teamMember.FacebookLink,
                       LinkedInLink = teamMember.LinkedInLink,
                       FullName = teamMember.FullName,
                       MemberType = teamMember.Type.FromDomain()
                   };
        }
    }
}