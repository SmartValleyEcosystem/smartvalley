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

        [MaxLength(100)]
        public string Country { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProjectArea { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        public byte HrEstimatesCount { get; set; }

        public byte LawyerEstimatesCount { get; set; }

        public byte AnalystEstimatesCount { get; set; }

        public byte TechnicalEstimatesCount { get; set; }

        public double? Score { get; set; }

        public void IncrementEstimatesCounter(ExpertiseArea expertiseArea)
        {
            switch (expertiseArea)
            {
                case ExpertiseArea.Hr:
                    HrEstimatesCount++;
                    break;
                case ExpertiseArea.Analyst:
                    AnalystEstimatesCount++;
                    break;
                case ExpertiseArea.Tech:
                    TechnicalEstimatesCount++;
                    break;
                case ExpertiseArea.Lawyer:
                    LawyerEstimatesCount++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(expertiseArea), expertiseArea, null);
            }
        }

        public byte GetEstimatesCounterValue(ExpertiseArea expertiseArea)
        {
            switch (expertiseArea)
            {
                case ExpertiseArea.Hr:
                    return HrEstimatesCount;
                case ExpertiseArea.Analyst:
                    return AnalystEstimatesCount;
                case ExpertiseArea.Tech:
                    return TechnicalEstimatesCount;
                case ExpertiseArea.Lawyer:
                    return LawyerEstimatesCount;
                default:
                    throw new ArgumentOutOfRangeException(nameof(expertiseArea), expertiseArea, null);
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