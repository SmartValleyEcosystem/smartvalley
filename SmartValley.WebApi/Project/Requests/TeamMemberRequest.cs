using System.ComponentModel.DataAnnotations;
 
 namespace SmartValley.WebApi.Project.Requests
 {
     public class TeamMemberRequest
     {
         [Required]
         [MaxLength(50)]
         public string FullName { get; set; }
 
         [Url]
         [MaxLength(100)]
         public string FacebookLink { get; set; }
 
         [Url]
         [MaxLength(100)]
         public string LinkedInLink { get; set; }
 
         public MemberType PersonType { get; set; }
     }
 }