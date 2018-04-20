using System.Collections.Generic;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ScoringStatisticsInArea
    {
        public ScoringStatisticsInArea(double? score,
                                       long requiredExpertsCount,
                                       IReadOnlyCollection<ExpertScoring> scorings,
                                       IReadOnlyCollection<ScoringOffer> offers,
                                       AreaType areaType)
        {
            Score = score;
            RequiredExpertsCount = requiredExpertsCount;
            Scorings = scorings;
            Offers = offers;
            AreaType = areaType;
        }

        public double? Score { get; }

        public long RequiredExpertsCount { get; }

        public IReadOnlyCollection<ExpertScoring> Scorings { get; }

        public IReadOnlyCollection<ScoringOffer> Offers { get; }

        public AreaType AreaType { get; }
    }
}