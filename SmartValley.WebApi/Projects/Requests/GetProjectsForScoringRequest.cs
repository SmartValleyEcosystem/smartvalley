using SmartValley.WebApi.Estimates;
using SmartValley.WebApi.Experts;

namespace SmartValley.WebApi.Projects.Requests
{
    public class GetProjectsForScoringRequest
    {
        public ExpertiseAreaType ExpertiseArea { get; set; }
    }
}