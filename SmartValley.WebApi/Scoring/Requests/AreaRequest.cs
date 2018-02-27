using SmartValley.WebApi.Experts;

namespace SmartValley.WebApi.Scoring.Requests
{
    public class AreaRequest
    {
        public AreaType Area { get; set; }
        
        public int ExpertsCount { get; set; }
    }
}