using System.Collections.Generic;
using System.Linq;
using SmartValley.Domain.Entities;
using AreaType = SmartValley.WebApi.Experts.AreaType;

namespace SmartValley.WebApi.Estimates.Responses
{
    public class AreaScoringCriteriaResponse
    {
        public AreaType Area { get; set; }

        public IReadOnlyCollection<ScoringCriteriaGroupResponse> Groups { get; set; }

        public static AreaScoringCriteriaResponse Create(AreaType area, IReadOnlyCollection<ScoringCriterion> criteria)
        {
            return new AreaScoringCriteriaResponse
                   {
                       Area = area,
                       Groups = criteria
                                .GroupBy(c => c.GroupKey)
                                .Select(g => ScoringCriteriaGroupResponse.Create(g.Key, g.ToArray()))
                                .ToArray()
                   };
        }
    }
}