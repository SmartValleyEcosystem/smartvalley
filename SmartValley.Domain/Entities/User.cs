using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class User
    {
        public long Id { get; set; }
        
        [Required]
        [MaxLength(42)]
        public Address Address { get; set; }

        [Required]
        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string SecondName { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public bool CanCreatePrivateProjects { get; set; }

        public DateTime? BirthDate { get; set; }

        public Sex? Sex { get; set; }

        public long? CountryId { get; set; }

        public string City { get; set; }

        public string LinkedInLink { get; set; }

        public string FacebookLink { get; set; }

        public string BitcointalkLink { get; set; }

        public string About { get; set; }

        public Country Country { get; set; }

        public Expert Expert { get; set; }

        public IEnumerable<Project> Projects { get; set; }

        public IEnumerable<UserRole> UserRoles { get; set; }
    }
}