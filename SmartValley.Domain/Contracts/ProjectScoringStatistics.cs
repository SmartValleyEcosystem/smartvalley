using System.Collections.Generic;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Contracts
{
    public class ProjectScoringStatistics
    {
        public ProjectScoringStatistics(double? score, IDictionary<AreaType, double?> areaScores)
        {
            Score = score;
            AreaScores = areaScores;
        }

        public double? Score { get; }

        public IDictionary<AreaType, double?> AreaScores { get; }
    }
}