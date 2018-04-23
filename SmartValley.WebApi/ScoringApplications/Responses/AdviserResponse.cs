using SmartValley.Domain;

namespace SmartValley.WebApi.ScoringApplications.Responses
{
    public class AdviserResponse
    {
        public string About { get; set; }

        public string FullName { get; set; }

        public string Reason { get; set; }

        public string FacebookLink { get; set; }

        public string LinkedInLink { get; set; }

        public static AdviserResponse Create(ScoringApplicationAdviser adviser)
        {
            return new AdviserResponse
                   {
                       LinkedInLink = adviser.LinkedInLink,
                       FacebookLink = adviser.FacebookLink,
                       About = adviser.About,
                       FullName = adviser.FullName,
                       Reason = adviser.Reason
                   };
        }
    }
}