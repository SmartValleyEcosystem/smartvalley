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
        public Address VotingAddress { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }
    }
}