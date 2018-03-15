using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class Project : IEntityWithId
    {
        public long Id { get; set; }

        [Required]
        public Guid ExternalId { get; set; }

        [Required]
        public long CountryId { get; set; }

        [Required]
        public CategoryType CategoryId { get; set; }

        [Required]
        public StageType StageId { get; set; }

        [Required]
        [MaxLength(42)]
        public Address AuthorAddress { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        public Country Country { get; set; }

        public Category Category { get; set; }

        public Stage Stage { get; set; }

        public IEnumerable<ProjectSocialMedia> ProjectSocialMedias { get; set; }
    }
}