using System.Collections.Generic;
using System.Linq;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Scorings.Responses
{
    public class ScoringResponse
    {
        public ScoringResponse()
        {
            Offers = new List<ScoringOfferAreaResponse>();
        }

        public long? Id { get; set; }

        public Domain.ScoringStatus ScoringStatus { get; set; }

        public double? Score { get; set; }

        public long RequiredExpertsCount { get; set; }

        public IEnumerable<ScoringOfferAreaResponse> Offers { get; set; }

        public static ScoringResponse FromScoring(Scoring scoring)
        {
            if (scoring == null)
            {
                return new ScoringResponse
                       {
                           ScoringStatus = Domain.ScoringStatus.Pending
                       };
            }

            return new ScoringResponse
                   {
                       ScoringStatus = scoring.Status,
                       Id = scoring.Id,
                       Score = scoring.Score,
                       RequiredExpertsCount = scoring.AreaScorings.Sum(x => x.ExpertsCount),
                       Offers = scoring.ScoringOffers.Select(ScoringOfferAreaResponse.FromDomain)
                   };
        }
    }
}