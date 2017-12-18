using System.ComponentModel.DataAnnotations;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class EstimateComment : IEntityWithId
    {
        public long Id { get; set; }

        public long ProjectId { get; set; }

        [Required]
        [MaxLength(42)]
        public string ExpertAddress { get; set; }

        public long QuestionId { get; set; }

        // TODO Remove when scores are migrated to the blockchain
        public int Score { get; set; }

        [Required]
        public string Comment { get; set; }
        
        public Project Project { get; set; }
        
        public Question Question { get; set; }
    }
}