using System.ComponentModel.DataAnnotations;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class Application : IEntityWithId
    {
        public long Id { get; set; }

        public virtual Project Project { get; set; }

        public long ProjectId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public decimal SoftCap { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public decimal HardCap { get; set; }

        [Url]
        [MaxLength(100)]
        public string FinancialModelLink { get; set; }

        public bool InvestmentsAreAttracted { get; set; }

        [MaxLength(100)]
        public string MVPLink { get; set; }

        [MaxLength(20)]
        public string CryptoCurrency { get; set; }

        [MaxLength(30)]
        public string ProjectStatus { get; set; }

        [Url]
        [MaxLength(100)]
        public string WhitePaperLink { get; set; }
    }
}