using System.Collections.Generic;
using SmartValley.Domain.Entities;

namespace SmartValley.Application.Contracts.Scorings
{
    public class ProjectScoringStatistics
    {
        public ProjectScoringStatistics(int? score, IReadOnlyCollection<AreaType> scoredAreas)
        {
            Score = score;
            ScoredAreas = scoredAreas;
        }

        public int? Score { get; }
        
        public IReadOnlyCollection<AreaType> ScoredAreas { get; }
    }
}