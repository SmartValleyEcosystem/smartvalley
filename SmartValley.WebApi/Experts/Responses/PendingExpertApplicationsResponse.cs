using System;
using System.Collections.Generic;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Experts.Responses
{
    public class PendingExpertApplicationsResponse
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTimeOffset ApplyDate { get; set; }

        public static PendingExpertApplicationsResponse Create(ExpertApplication expertApplication)
        {
            return new PendingExpertApplicationsResponse
                   {
                       Id = expertApplication.Id,
                       ApplyDate = expertApplication.ApplyDate,
                       FirstName = expertApplication.FirstName,
                       LastName = expertApplication.LastName
                   };
        }
    }
}