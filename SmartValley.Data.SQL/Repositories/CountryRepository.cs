using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class CountryRepository : EntityCrudRepository<Country>, ICountryRepository
    {
        public CountryRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
            : base(readContext, editContext)
        {
        }

        public Task<Country> GetByCodeAsync(string code)
            => ReadContext.Countries.FirstOrDefaultAsync(i => i.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
    }
}