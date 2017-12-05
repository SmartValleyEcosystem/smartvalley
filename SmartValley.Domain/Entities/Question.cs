using System.ComponentModel.DataAnnotations;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class Question : IEntityWithId
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        public ExpertiseArea ExpertiseArea { get; set; }

        public int MinScore { get; set; }

        public int MaxScore { get; set; }
    }
}