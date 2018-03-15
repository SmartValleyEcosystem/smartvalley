using System.ComponentModel.DataAnnotations;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class EstimateComment : IEntityWithId
    {
        public long Id { get; set; }
        
        public long ProjectId { get; set; }
        
        public long ExpertId { get; set; }
        
        public long QuestionId { get; set; }

        [Required]
        public string Comment { get; set; }

        public Project Project { get; set; }

        public Question Question { get; set; }

        public Expert Expert { get; set; }
    }
}