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

        public bool IsScoredByHr { get; set; }

        public bool IsScoredByLawyer { get; set; }

        public bool IsScoredByAnalyst { get; set; }

        public bool IsScoredByTechnical { get; set; }

        public double? Score { get; set; }
    }
}