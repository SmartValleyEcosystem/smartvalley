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
    }
}