using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class Country: IEntityWithId
    {
        public long Id { get; set; }

        [Required, MaxLength(2)]
        public string Code { get; set; }
        
        public ICollection<Project> Projects { get; set; }
        public ICollection<ScoringApplication> ScoringApplications { get; set; }
    }
}