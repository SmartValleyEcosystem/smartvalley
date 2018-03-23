using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class Project : IEntityWithId
    {
        public long Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public Guid ExternalId { get; set; }

        [Required]
        public long CountryId { get; set; }

        [Required]
        public CategoryType CategoryId { get; set; }

        [Required]
        public StageType StageId { get; set; }

        [Required]
        public long AuthorId { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [MaxLength(200)]
        public string Website { get; set; }

        [MaxLength(200)]
        public string WhitePaperLink { get; set; }

        public DateTimeOffset? IcoDate { get; set; }

        [Url, MaxLength(50)]
        public string ContactEmail { get; set; }

        [Url, MaxLength(200)]
        public string ImageUrl { get; set; }

        public SocialNetworks SocialNetworks { get; set; }

        public User Author { get; set; }

        public Country Country { get; set; }

        public Category Category { get; set; }

        public Stage Stage { get; set; }
    }
}