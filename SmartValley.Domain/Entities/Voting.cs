using System;
using System.ComponentModel.DataAnnotations;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class Voting : IEntityWithId
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(42)]
        public string VotingAddress { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}