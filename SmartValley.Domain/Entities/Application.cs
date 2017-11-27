using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class Application : IEntityWithId
    {
        public long Id { get; set; }

        public virtual Project Project { get; set; }

        public long ProjectId { get; set; }


        [MaxLength(40)]
        public string SoftCap { get; set; }

        [MaxLength(40)]
        public string HardCap { get; set; }

        [Url]
        [MaxLength(400)]
        public string FinancialModelLink { get; set; }

        public bool InvestmentsAreAttracted { get; set; }

        [MaxLength(400)]
        public string MvpLink { get; set; }

        [MaxLength(100)]
        public string BlockchainType { get; set; }

        [MaxLength(100)]
        public string ProjectStatus { get; set; }

        [Url]
        [MaxLength(400)]
        public string WhitePaperLink { get; set; }
    }
}