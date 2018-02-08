using System;
using System.ComponentModel.DataAnnotations;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class Scoring : IEntityWithId
    {
        public long Id { get; set; }

        public long ProjectId { get; set; }

        [Required]
        [MaxLength(42)]
        public string ContractAddress { get; set; }

        public bool IsScoredByHr { get; set; }

        public bool IsScoredByLawyer { get; set; }

        public bool IsScoredByAnalyst { get; set; }

        public bool IsScoredByTechnical { get; set; }

        public double? Score { get; set; }

        public Project Project { get; set; }

        public bool IsCompletedInArea(ExpertiseAreaType areaType)
        {
            switch (areaType)
            {
                case ExpertiseAreaType.Hr:
                    return IsScoredByHr;
                case ExpertiseAreaType.Analyst:
                    return IsScoredByAnalyst;
                case ExpertiseAreaType.Tech:
                    return IsScoredByTechnical;
                case ExpertiseAreaType.Lawyer:
                    return IsScoredByLawyer;
                default:
                    throw new ArgumentOutOfRangeException(nameof(areaType), areaType, null);
            }
        }
    }
}