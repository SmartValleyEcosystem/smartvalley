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
    }
}