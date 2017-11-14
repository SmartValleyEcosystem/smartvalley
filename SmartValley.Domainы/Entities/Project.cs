using System.ComponentModel.DataAnnotations;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class Project: IEntityWithId
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string AuthorLogin { get; set; }

        [Required]
        [MaxLength(42)]
        public string AuthorAddress { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(30)]
        public string Country { get; set; }

        [Required]
        [MaxLength(20)]
        public string ProjectArea { get; set; }

        [MaxLength(255)]
        public string ProblemDesc { get; set; }

        [MaxLength(255)]
        public string SolutionDesc { get; set; }
    }
}
