using System.Collections.Generic;
using System.Linq;
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
        
        private IReadOnlyCollection<AreaType> ScoredAreas { get; }

        public bool IsScoredByHr() => ScoredAreas.Any(a => a == AreaType.Hr);

        public bool IsScoredByAnalyst() => ScoredAreas.Any(a => a == AreaType.Analyst);

        public bool IsScoredByTech() => ScoredAreas.Any(a => a == AreaType.Tech);

        public bool IsScoredByLawyer() => ScoredAreas.Any(a => a == AreaType.Lawyer);
    }
}