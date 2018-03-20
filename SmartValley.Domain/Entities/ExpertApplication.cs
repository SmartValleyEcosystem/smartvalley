using System;
using System.ComponentModel.DataAnnotations;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class ExpertApplication : IEntityWithId
    {
        public long Id { get; set; }

        public long ApplicantId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTimeOffset ApplyDate { get; set; }

        public Sex Sex { get; set; }

        [Required]
        [MaxLength(100)]
        public string Country { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(400)]
        public string LinkedInLink { get; set; }

        [MaxLength(400)]
        public string FacebookLink { get; set; }

        [Required]
        [MaxLength(1500)]
        public string Description { get; set; }

        [Required]
        [MaxLength(1500)]
        public string Why { get; set; }

        public DocumentType DocumentType { get; set; }

        [MaxLength(30)]
        public string DocumentNumber { get; set; }

        [Url, MaxLength(200)]
        public string ScanUrl { get; set; }

        [Url, MaxLength(200)]
        public string PhotoUrl { get; set; }

        [Url, MaxLength(200)]
        public string CvUrl { get; set; }

        public ExpertApplicationStatus Status { get; set; }

        public User Applicant { get; set; }
    }
}