using System;
using System.ComponentModel.DataAnnotations;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class Project : IEntityWithId
    {
        public long Id { get; set; }

        [Required]
        public Guid ExternalId { get; set; }

        [Required]
        [MaxLength(42)]
        public string AuthorAddress { get; set; }

        [Required]
        [MaxLength(42)]
        public string ProjectAddress { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(30)]
        public string Country { get; set; }

        [Required]
        [MaxLength(20)]
        public string ProjectArea { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public byte HrEstimatesCount { get; set; }

        public byte LawyerEstimatesCount { get; set; }

        public byte AnalystEstimatesCount { get; set; }

        public byte TechnicalEstimatesCount { get; set; }

        public double? Score { get; set; }

        public void IncrementEstimatesCounter(ScoringCategory scoringCategory)
        {
            switch (scoringCategory)
            {
                case ScoringCategory.Hr:
                    HrEstimatesCount++;
                    break;
                case ScoringCategory.Analyst:
                    AnalystEstimatesCount++;
                    break;
                case ScoringCategory.Tech:
                    TechnicalEstimatesCount++;
                    break;
                case ScoringCategory.Lawyer:
                    LawyerEstimatesCount++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(scoringCategory), scoringCategory, null);
            }
        }

        public byte GetEstimatesCounterValue(ScoringCategory scoringCategory)
        {
            switch (scoringCategory)
            {
                case ScoringCategory.Hr:
                    return HrEstimatesCount;
                case ScoringCategory.Analyst:
                    return AnalystEstimatesCount;
                case ScoringCategory.Tech:
                    return TechnicalEstimatesCount;
                case ScoringCategory.Lawyer:
                    return LawyerEstimatesCount;
                default:
                    throw new ArgumentOutOfRangeException(nameof(scoringCategory), scoringCategory, null);
            }
        }

        public bool IsReadyForScoring(byte requiredEstimatesCount)
        {
            return HrEstimatesCount == requiredEstimatesCount
                   && LawyerEstimatesCount == requiredEstimatesCount
                   && AnalystEstimatesCount == requiredEstimatesCount
                   && TechnicalEstimatesCount == requiredEstimatesCount;
        }
    }
}