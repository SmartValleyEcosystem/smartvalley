using System.Collections.Generic;
using SmartValley.Domain.Entities;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.EtheriumTransactions.Requests
{
    public class EtheriumTransactionsQueryRequest : CollectionPageRequest
    {
        public IReadOnlyCollection<long> UserIds { get; set; }

        public IReadOnlyCollection<long> EntityIds { get; set; }

        public IReadOnlyCollection<EthereumTransactionEntityType> EntityTypes { get; set; }

        public IReadOnlyCollection<EthereumTransactionType> TransactionTypes { get; set; }

        public IReadOnlyCollection<EthereumTransactionStatus> Statuses { get; set; }
    }
}