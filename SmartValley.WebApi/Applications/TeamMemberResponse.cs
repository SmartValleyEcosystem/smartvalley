using SmartValley.WebApi.TeamMembers;

namespace SmartValley.WebApi.Applications
{
    public class TeamMemberResponse
    {
        public Type MemberType { get; set; }

        public string FullName { get; set; }

        public string FacebookLink { get; set; }

        public string LinkedInLink { get; set; }
    }
}