using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SmartValley.Domain.Entities
{
    public class ExpertApplication
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

        public long CountryId { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(400)]
        public string LinkedInLink { get; set; }

        [MaxLength(400)]
        public string FacebookLink { get; set; }

        [MaxLength(400)]
        public string BitcointalkLink { get; set; }

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

        public ICollection<ExpertApplicationArea> ExpertApplicationAreas { get; set; }

        public void SetAreas(IReadOnlyCollection<int> areas)
        {
            foreach (var areaType in areas)
            {
                ExpertApplicationAreas.Add(new ExpertApplicationArea
                                           {
                                               AreaId = (AreaType) areaType
                                           });
            }
        }

        public void SetAccepted(IReadOnlyCollection<int> areas)
        {
            Status = ExpertApplicationStatus.Accepted;

            foreach (var area in ExpertApplicationAreas)
            {
                area.Status = areas.Contains((int) area.AreaId) ? ExpertApplicationStatus.Accepted : ExpertApplicationStatus.Rejected;
            }
        }

        public void SetRejected()
        {
            Status = ExpertApplicationStatus.Rejected;

            foreach (var area in ExpertApplicationAreas)
            {
                area.Status = ExpertApplicationStatus.Rejected;
            }
        }
    }
}