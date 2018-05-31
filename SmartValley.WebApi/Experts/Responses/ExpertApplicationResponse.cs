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

        public static ExpertApplicationResponse Create(ExpertApplication application, User applicant, Country country)
        {
            return new ExpertApplicationResponse
                   {
                       Id = application.Id,
                       Address = applicant.Address,
                       DocumentType = application.DocumentType.FromDomain(),
                       Sex = application.Sex,
                       Areas = application.ExpertApplicationAreas.Select(s => (int) s.AreaId).ToArray(),
                       Why = application.Why,
                       LastName = application.LastName,
                       LinkedInLink = application.LinkedInLink,
                       City = application.City,
                       FirstName = application.FirstName,
                       DocumentNumber = application.DocumentNumber,
                       BirthDate = application.BirthDate,
                       FacebookLink = application.FacebookLink,
                       Description = application.Description,
                       CvUrl = application.CvUrl,
                       PhotoUrl = application.PhotoUrl,
                       ScanUrl = application.ScanUrl,
                       CountryIsoCode = country.Code
                   };
        }
    }
}