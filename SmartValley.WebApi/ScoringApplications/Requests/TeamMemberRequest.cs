namespace SmartValley.WebApi.ScoringApplications.Requests
{
    public class TeamMemberRequest
    {
        public string FullName { get; set; }

        public string ProjectRole { get; set; }

        public string About { get; set; }

        public string FacebookLink { get; set; }

        public string LinkedInLink { get; set; }

        public string AdditionalInformation { get; set; }
    }
}