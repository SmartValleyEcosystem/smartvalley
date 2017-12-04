using System.ComponentModel.DataAnnotations;
using SmartValley.WebApi.Estimates;

namespace SmartValley.WebApi.Scoring
{
    public class GetProjectsForScoringRequest
    {
        public ExpertiseAreaApi ExpertiseAreaApi { get; set; }

        [Required]
        public string ExpertAddress { get; set; }
    }
}