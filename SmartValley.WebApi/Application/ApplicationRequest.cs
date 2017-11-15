namespace SmartValley.WebApi.Application
{
    public class ApplicationRequest
    {
        public string Name { get; set; }
        public string AuthorAddress { get; set; }
        
        public string ProjectArea { get; set; }

        public string ProbDesc { get; set; }

        public string SolDesc { get; set; }
        
        public string ProjStat { get; set; }
        
        public string WPLink { get; set; }
        
        public string BlockChainType { get; set; }
        
        public string Country { get; set; }
        
        public string FinModelLink { get; set; }

        public string MvpLink { get; set; }
        
        public string SoftCap { get; set; }
        
        public string HardCap { get; set; }
        
        public bool AttractInv { get; set; }


        public TeamMemberModel CEO { get; set; }

        public TeamMemberModel CFO { get; set; }

        public TeamMemberModel CMO { get; set; }

        public TeamMemberModel CTO { get; set; }

        public TeamMemberModel PR { get; set; }
    }
}
