using System;
using System.Collections.Generic;

namespace SmartValley.Domain.Entities
{
    public class Application
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid CountryId { get; set; }
        public Guid CryptoCurrencyId { get; set; }

        public decimal SoftCap { get; set; }

        public decimal HardCap { get; set; }

        public string FinancialModelLink { get; set; }

        public bool InvestmentsAreAttracted { get; set; }

        public string MVPLink { get; set; }

        public virtual CryptoCurrency CryptoCurrency { get; set; }
        public virtual Country Country { get; set; }
        public virtual IEnumerable<PersonApplication> TeamPersons { get; set; }
        public virtual Project Project { get; set; }

    }
}
