using System.Collections.Generic;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Contracts
{
    public class ScoringResults
    {
        public ScoringResults(double score, IDictionary<AreaType, double> areaScores)
        {
            Score = score;
            AreaScores = areaScores;
        }

        public double Score { get; }

        public IDictionary<AreaType, double> AreaScores { get; }
    }
}