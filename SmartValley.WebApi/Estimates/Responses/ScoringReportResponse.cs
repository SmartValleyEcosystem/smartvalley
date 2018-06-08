using System.Collections.Generic;
using SmartValley.WebApi.Experts.Responses;

namespace SmartValley.WebApi.Estimates.Responses
{
    public class ScoringReportResponse
    {
        public IReadOnlyCollection<ExpertResponse> Experts { get; set; }
        
        public IReadOnlyCollection<ScoringReportInAreaResponse> ScoringReportsInArea { get; set; }
    }
}