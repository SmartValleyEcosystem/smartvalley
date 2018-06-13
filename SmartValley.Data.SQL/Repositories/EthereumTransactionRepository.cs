using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EthereumTransactionRepository : IEthereumTransactionRepository
    {
        private readonly IEditableDataContext _editContext;

        public EthereumTransactionRepository(IEditableDataContext editContext)
        {
            _editContext = editContext;
        }

        public Task<EthereumTransaction> GetByHashAsync(string hash)
            => _editContext.EthereumTransactions.FirstOrDefaultAsync(t => t.Hash.Equals(hash, StringComparison.OrdinalIgnoreCase));

        public Task SaveChangesAsync()
            => _editContext.SaveAsync();

        public void Add(EthereumTransaction ethereumTransaction)
        {
            _editContext.EthereumTransactions.Add(ethereumTransaction);
        }

        public async Task<IReadOnlyCollection<EthereumTransaction>> GetByAllotmentEventIdAsync(long allotmentEventId)
        {
            return await _editContext.EthereumTransactions
                                             .Where(x => x.AllotmentEventId == allotmentEventId)
                                             .ToArrayAsync();
        }
    }
}