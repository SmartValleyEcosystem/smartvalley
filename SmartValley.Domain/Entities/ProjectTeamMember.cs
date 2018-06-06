using System.ComponentModel.DataAnnotations;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class ProjectTeamMember : Entity, IUpdateble<ProjectTeamMember>
    {
        // ReSharper disable once UnusedMember.Local
        private ProjectTeamMember()
        {
            
        }

        public ProjectTeamMember(long id, string fullName, string role, string about, string facebook, string linkedin)
        {
            Id = id;
            FullName = fullName;
            Role = role;
            About = about;
            Facebook = facebook;
            Linkedin = linkedin;
        }

        public long ProjectId { get; private set; }

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Role { get; set; }

        [MaxLength(500)]
        public string About { get; set; }

        [Url, MaxLength(200)]
        public string PhotoUrl { get; set; }

        [Url, MaxLength(200)]
        public string Facebook { get; set; }

        [Url, MaxLength(200)]
        public string Linkedin { get; set; }

        public void Update(ProjectTeamMember member)
        {
            About = member.About;
            Facebook = member.Facebook;
            FullName = member.FullName;
            Linkedin = member.Linkedin;
            Role = member.Role;
        }
    }
}