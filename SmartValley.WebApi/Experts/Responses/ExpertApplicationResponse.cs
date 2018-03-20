using System;
using System.Collections.Generic;
using System.Linq;
using SmartValley.Domain;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Experts.Responses
{
    public class ExpertApplicationResponse
    {
        public long Id { get; set; }

        public string Address { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public Sex Sex { get; set; }

        public string CountryIsoCode { get; set; }

        public string City { get; set; }

        public string LinkedInLink { get; set; }

        public string FacebookLink { get; set; }

        public string Description { get; set; }

        public string Why { get; set; }

        public DocumentType DocumentType { get; set; }

        public string DocumentNumber { get; set; }

        public IReadOnlyCollection<int> Areas { get; set; }

        public string ScanUrl { get; set; }

        public string PhotoUrl { get; set; }

        public string CvUrl { get; set; }

        public static ExpertApplicationResponse Create(ExpertApplicationDetails applicationDetails)
        {
            return new ExpertApplicationResponse
                   {
                       Id = applicationDetails.ExpertApplication.Id,
                       Address = applicationDetails.Address,
                       DocumentType = applicationDetails.ExpertApplication.DocumentType.FromDomain(),
                       Sex = applicationDetails.ExpertApplication.Sex,
                       Areas = applicationDetails.Areas.Select(s => (int) s.Id).ToArray(),
                       Why = applicationDetails.ExpertApplication.Why,
                       LastName = applicationDetails.ExpertApplication.LastName,
                       LinkedInLink = applicationDetails.ExpertApplication.LinkedInLink,
                       City = applicationDetails.ExpertApplication.City,
                       FirstName = applicationDetails.ExpertApplication.FirstName,
                       DocumentNumber = applicationDetails.ExpertApplication.DocumentNumber,
                       BirthDate = applicationDetails.ExpertApplication.BirthDate,
                       FacebookLink = applicationDetails.ExpertApplication.FacebookLink,
                       Description = applicationDetails.ExpertApplication.Description,
                       CvUrl = applicationDetails.ExpertApplication.CvUrl,
                       PhotoUrl = applicationDetails.ExpertApplication.PhotoUrl,
                       ScanUrl = applicationDetails.ExpertApplication.ScanUrl,
                       CountryIsoCode = applicationDetails.ExpertApplication.Country
                   };
        }
    }
}