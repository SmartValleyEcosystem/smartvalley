using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartValley.Domain.Entities
{
    public class Area
    {
        public AreaType Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int MaxScore { get; set; }

        public IEnumerable<ExpertApplicationArea> ExpertApplicationAreas { get; set; }
    }
}