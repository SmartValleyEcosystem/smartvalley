using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartValley.WebApi.Applications
{
    public class ApplicationResponse
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
        
        public decimal SoftCap { get; set; }

        public decimal HardCap { get; set; }

        public bool AttractedInvestments { get; set; }

        public IReadOnlyCollection<TeamMemberResponse> TeamMembers { get; set; }
    }
}