using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Experts.Requests
{
    public class CreateExpertApplicationRequest
    {
        [Required]
        public string TransactionHash { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public Sex Sex { get; set; }

        [Required]
        [MaxLength(100)]
        public string CountryIsoCode { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(400)]
        public string LinkedInLink { get; set; }

        [MaxLength(400)]
        public string FacebookLink { get; set; }

        [MaxLength(1500)]
        public string Description { get; set; }

        [Required]
        [MaxLength(1500)]
        public string Why { get; set; }

        public DocumentType DocumentType { get; set; }

        [MaxLength(30)]
        public string DocumentNumber { get; set; }

        public IReadOnlyCollection<int> Areas { get; set; }
    }
}