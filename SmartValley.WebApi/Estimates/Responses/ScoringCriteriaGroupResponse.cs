using System.Collections.Generic;
using System.Linq;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Estimates.Responses
{
    public class ScoringCriteriaGroupResponse
    {
        public string Key { get; set; }

        public int Order { get; set; }

        public IReadOnlyCollection<ScoringCriterionResponse> Criteria { get; set; }

        public static ScoringCriteriaGroupResponse Create(string key, IReadOnlyCollection<ScoringCriterion> criteria)
        {
            return new ScoringCriteriaGroupResponse
                   {
                       Key = key,
                       Order = criteria.First().GroupOrder,
                       Criteria = criteria.Select(ScoringCriterionResponse.From).ToArray()
                   };
        }
    }
}