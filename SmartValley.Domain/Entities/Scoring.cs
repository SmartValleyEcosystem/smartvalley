using System;
using System.Collections.Generic;
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
        public Address ContractAddress { get; set; }

        public double? Score { get; set; }

        public DateTimeOffset? ScoringStartDate { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public DateTimeOffset OffersEndDate { get; set; }

        public DateTimeOffset? ScoringEndDate { get; set; }

        public Project Project { get; set; }

        public IEnumerable<AreaScoring> AreaScorings { get; set; }
    }
}