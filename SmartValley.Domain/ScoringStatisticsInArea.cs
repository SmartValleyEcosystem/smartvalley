using System.Collections.Generic;

namespace SmartValley.Domain
{
    public class ScoringStatisticsInArea
    {
        public ScoringStatisticsInArea(double? score, IReadOnlyCollection<Estimate> estimates)
        {
            Score = score;
            Estimates = estimates;
        }

        public double? Score { get; }

        public IReadOnlyCollection<Estimate> Estimates { get; }

        public static ScoringStatisticsInArea Empty => new ScoringStatisticsInArea(null, new Estimate[0]);
    }
}