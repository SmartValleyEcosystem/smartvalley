using System.Collections.Generic;
using SmartValley.Domain.Entities;

namespace SmartValley.Application.Contracts.Scorings
{
    public class ProjectScoringStatistics
    {
        public ProjectScoringStatistics(int? score, IDictionary<AreaType, double?> areaScores)
        {
            Score = score;
            AreaScores = areaScores;
        }

        public int? Score { get; }

        public IDictionary<AreaType, double?> AreaScores { get; }
    }
}