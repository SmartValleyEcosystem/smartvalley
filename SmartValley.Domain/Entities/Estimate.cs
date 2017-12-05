using System.ComponentModel.DataAnnotations;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class Estimate : IEntityWithId
    {
        public long Id { get; set; }

        public Project Project { get; set; }

        public long ProjectId { get; set; }

        [Required]
        [MaxLength(42)]
        public string ExpertAddress { get; set; }

        public Question Question { get; set; }

        public long QuestionId { get; set; }

        public int Score { get; set; }

        [Required]
        public string Comment { get; set; }
    }
}