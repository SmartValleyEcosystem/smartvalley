using System;
using System.Collections.Generic;
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
      
        [MaxLength(50)]
        public string ScanName { get; set; }
      
        [MaxLength(50)]
        public string PhotoName { get; set; }
    
        [MaxLength(50)]
        public string CvName { get; set; }

        public User Applicant { get; set; }
    }
}