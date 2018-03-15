using System.Collections.Generic;

namespace SmartValley.Domain
{
    public class ScoringStatisticsInArea
    {
        public ScoringStatisticsInArea(double? averageScore, IReadOnlyCollection<Estimate> estimates)
        {
            AverageScore = averageScore;
            Estimates = estimates;
        }

        public double? AverageScore { get; }

        public IReadOnlyCollection<Estimate> Estimates { get; }

        public static ScoringStatisticsInArea Empty => new ScoringStatisticsInArea(null, new Estimate[0]);
    }
}