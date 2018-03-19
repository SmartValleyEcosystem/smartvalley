using System.Collections.Generic;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SmartValley.WebApi.Applications.Requests
{
    public class ApplicationRequest
    {
        public string Name { get; set; }

        public string AuthorAddress { get; set; }

        public int CategoryType { get; set; }

        public string ProjectId { get; set; }

        public string Description { get; set; }

        public string ProjectStatus { get; set; }

        public string WhitePaperLink { get; set; }

        public string BlockChainType { get; set; }

        public string CountryCode { get; set; }

        public string FinanceModelLink { get; set; }

        public string MvpLink { get; set; }

        public string SoftCap { get; set; }

        public string HardCap { get; set; }

        public bool AttractedInvestments { get; set; }

        public IEnumerable<ApplicationTeamMemberRequest> TeamMembers { get; set; }
    }
}