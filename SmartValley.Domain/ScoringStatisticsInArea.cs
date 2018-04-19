using System.Collections.Generic;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ScoringStatisticsInArea
    {
        public ScoringStatisticsInArea(double? score,
                                       long requiredExpertsCount,
                                       IReadOnlyCollection<ExpertScoringConclusion> conclusions,
                                       IReadOnlyCollection<ScoringOffer> offers,
                                       AreaType areaType)
        {
            Score = score;
            RequiredExpertsCount = requiredExpertsCount;
            Conclusions = conclusions;
            Offers = offers;
            AreaType = areaType;
        }

        public double? Score { get; }

        public long RequiredExpertsCount { get; }

        public IReadOnlyCollection<ExpertScoringConclusion> Conclusions { get; }

        public IReadOnlyCollection<ScoringOffer> Offers { get; }

        public AreaType AreaType { get; }
    }
}