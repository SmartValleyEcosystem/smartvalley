using System.Collections.Generic;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ScoringStatisticsInArea
    {
        public ScoringStatisticsInArea(double? score, IReadOnlyCollection<Estimate> estimates, IReadOnlyCollection<ExpertScoringConclusion> conclusions,
                                       IReadOnlyCollection<ScoringOffer> offers)
        {
            Score = score;
            Estimates = estimates;
            Conclusions = conclusions;
            Offers = offers;
        }

        public double? Score { get; }

        public IReadOnlyCollection<Estimate> Estimates { get; }

        public IReadOnlyCollection<ExpertScoringConclusion> Conclusions { get; set; }

        public IReadOnlyCollection<ScoringOffer> Offers { get; set; }

        public static ScoringStatisticsInArea Empty => new ScoringStatisticsInArea(null, new Estimate[0], new ExpertScoringConclusion[0], new ScoringOffer[0]);
    }
}