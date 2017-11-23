using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.Applications
{
    public class ApplicationRequest
    {
        public string Name { get; set; }

        public string AuthorAddress { get; set; }

        public string ProjectArea { get; set; }

        public string ProjectId { get; set; }

        public string Description { get; set; }

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

        public string TransactionHash { get; set; }

        public IEnumerable<TeamMemberRequest> TeamMembers { get; set; }
    }
}