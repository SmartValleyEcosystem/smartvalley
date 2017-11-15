using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.Application
{
    public class ApplicationRequest
    {
        public string Name { get; set; }
        public string AuthorAddress { get; set; }

        public string ProjectArea { get; set; }

        public string ProbablyDescription { get; set; }

        public string SolutionDescription { get; set; }

        public string ProjectStatus { get; set; }

        public string WhitePaperLink { get; set; }

        public string BlockChainType { get; set; }

        public string Country { get; set; }

        public string FinanceModelLink { get; set; }

        public string MvpLink { get; set; }

        [Required]
        public string SoftCap { get; set; }

        [Required]
        public string HardCap { get; set; }

        public bool AttractedInvestnemts { get; set; }

        public IEnumerable<TeamMemberRequest> TeamMembers { get; set; }
    }
}