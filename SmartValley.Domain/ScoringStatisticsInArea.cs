using System.Collections.Generic;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ScoringStatisticsInArea
    {
        public ScoringStatisticsInArea(double? score,
                                       long requiredExpertsCount,
                                       IReadOnlyCollection<Estimate> estimates,
                                       IReadOnlyCollection<ExpertScoringConclusion> conclusions,
                                       IReadOnlyCollection<ScoringOffer> offers,
                                       AreaType areaType)
        {
            Score = score;
            RequiredExpertsCount = requiredExpertsCount;
            Estimates = estimates;
            Conclusions = conclusions;
            Offers = offers;
            AreaType = areaType;
        }

        public double? Score { get; }

        public long RequiredExpertsCount { get; }

        public IReadOnlyCollection<Estimate> Estimates { get; }

        public IReadOnlyCollection<ExpertScoringConclusion> Conclusions { get; }

        public IReadOnlyCollection<ScoringOffer> Offers { get; }

        public AreaType AreaType { get; }
    }
}