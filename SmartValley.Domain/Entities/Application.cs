using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class Application : IEntityWithId
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }

        public decimal SoftCap { get; set; }

        public decimal HardCap { get; set; }

        public string FinancialModelLink { get; set; }

        public bool InvestmentsAreAttracted { get; set; }

        public string MVPLink { get; set; }

        public string CryptoCurrency { get; set; }

        public string ProjectStatus { get; set; }

        public string WhitePaperLink { get; set; }
        
        //public virtual Project Project { get; set; }

    }
}
