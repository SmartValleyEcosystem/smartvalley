using System.ComponentModel.DataAnnotations;

namespace SmartValley.Domain.Entities
{
    public class ScoredProject
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(42)]
        public string ExpertAddress { get; set; }

        public ExpertiseArea ExpertiseArea { get; set; }

        public long ProjectId { get; set; }

        public virtual Project Project { get; set; }
    }
}