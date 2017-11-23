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
    }
}
