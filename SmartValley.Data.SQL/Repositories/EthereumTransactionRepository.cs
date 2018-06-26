using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Data.SQL.Extensions;
using SmartValley.Domain;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EthereumTransactionRepository : IEthereumTransactionRepository
    {
        private readonly IEditableDataContext _editContext;
        private readonly IReadOnlyDataContext _readContext;

        public EthereumTransactionRepository(IEditableDataContext editContext, IReadOnlyDataContext readContext)
        {
            _editContext = editContext;
            _readContext = readContext;
        }

        public async Task<PagingCollection<EthereumTransaction>> GetAsync(EtheriumTransactionsQuery query)
        {
            var querable = from transaction in _readContext.EthereumTransactions
                           where query.UserIds.Count == 0 || query.UserIds.Contains(transaction.User.Id)
                           where query.EntityIds.Count == 0 || query.EntityIds.Contains(transaction.EntityId)
                           where query.EntityTypes.Count == 0 || query.EntityTypes.Contains(transaction.EntityType)
                           where query.TransactionTypes.Count == 0 || query.TransactionTypes.Contains(transaction.TransactionType)
                           where query.Statuses.Count == 0 || query.Statuses.Contains(transaction.Status)
                           select transaction;

            return await querable.GetPageAsync(query.Offset, query.Count);
        }

        public Task<EthereumTransaction> GetByHashAsync(string hash)
            => _editContext.EthereumTransactions.FirstOrDefaultAsync(t => t.Hash.Equals(hash, StringComparison.OrdinalIgnoreCase));

        public void Add(EthereumTransaction ethereumTransaction)
            => _editContext.EthereumTransactions.Add(ethereumTransaction);

        public Task SaveChangesAsync()
            => _editContext.SaveAsync();
    }
}