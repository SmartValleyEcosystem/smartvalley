using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly IReadOnlyDataContext _readContext;

        public CountryRepository(IReadOnlyDataContext readContext)
        {
            _readContext = readContext;
        }

        public Task<Country> GetByCodeAsync(string code)
            => _readContext.Countries.FirstOrDefaultAsync(i => i.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
    }
}