using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.Projects.Requests
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ProjectTeamMemberRequest
    {
        public long Id { get; set; }

        [MaxLength(200)]
        public string FullName { get; set; }

        [MaxLength(100)]
        public string Role { get; set; }

        [MaxLength(500)]
        public string About { get; set; }

        [Url, MaxLength(200)]
        public string Facebook { get; set; }

        [Url, MaxLength(200)]
        public string Linkedin { get; set; }
    }
}