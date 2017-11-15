using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.Project.Requests
{
    public class CreateProjectRequest
    {
        [Required]
        [MaxLength(20)]
        public string AuthorAddress { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(30)]
        public string Country { get; set; }

        [Required]
        [MaxLength(20)]
        public string ProjectArea { get; set; }

        [MaxLength(255)]
        public string ProblemDescription { get; set; }

        [MaxLength(255)]
        public string SolutionDescription { get; set; }

        [Range(1, int.MaxValue)]
        public decimal SoftCap { get; set; }

        [Range(1, int.MaxValue)]
        public decimal HardCap { get; set; }

        [Url]
        [MaxLength(100)]
        public string FinancialModelLink { get; set; }

        public bool AreInvestmentsAttracted { get; set; }

        [MaxLength(100)]
        public string MvpLink { get; set; }

        [MaxLength(20)]
        public string CryptoCurrency { get; set; }

        [MaxLength(30)]
        public string ProjectStatus { get; set; }

        [Url]
        [MaxLength(100)]
        public string WhitePaperLink { get; set; }

        public IReadOnlyCollection<TeamMemberRequest> TeamMembers { get; set; }
    }
}