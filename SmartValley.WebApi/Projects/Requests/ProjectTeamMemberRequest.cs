using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using SmartValley.Domain.Entities;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Projects.Requests
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ProjectTeamMemberRequest
    {
        public long Id { get; set; }

        public string FullName { get; set; }

        public string Role { get; set; }

        public string About { get; set; }

        [Url, MaxLength(500)]
        public string Facebook { get; set; }

        [Url, MaxLength(500)]
        public string Linkedin { get; set; }
    }
}