using System;
using SmartValley.Domain.Entities;

namespace SmartValley.Application.Contracts.Project
{
    public class ProjectScoringStatistics
    {
        public int? Score { get; set; }

        public bool IsScoredByHr { get; set; }

        public bool IsScoredByAnalyst { get; set; }

        public bool IsScoredByTech { get; set; }

        public bool IsScoredByLawyer { get; set; }

        public bool IsScoredInArea(ExpertiseArea area)
        {
            switch (area)
            {
                case ExpertiseArea.Hr:
                    return IsScoredByHr;
                case ExpertiseArea.Analyst:
                    return IsScoredByAnalyst;
                case ExpertiseArea.Tech:
                    return IsScoredByTech;
                case ExpertiseArea.Lawyer:
                    return IsScoredByLawyer;
                default:
                    throw new ArgumentOutOfRangeException(nameof(area), area, null);
            }
        }
    }
}