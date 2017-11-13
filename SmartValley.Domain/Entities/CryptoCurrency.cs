using System;
using System.Collections.Generic;
using System.Text;

namespace SmartValley.Domain.Entities
{
    public class CryptoCurrency
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
    }
}
