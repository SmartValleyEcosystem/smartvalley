using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartValley.Domain.Entities
{
    public class ExpertiseArea
    {
        public ExpertiseAreaType Id { get; set; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<ExpertApplicationArea> ExpertApplicationAreas { get; set; }
    }
}