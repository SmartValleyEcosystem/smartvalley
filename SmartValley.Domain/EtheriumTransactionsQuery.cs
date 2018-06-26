using System;
using System.Collections.Generic;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class EtheriumTransactionsQuery
    {
        public EtheriumTransactionsQuery(int offset,
                                         int count,
                                         IReadOnlyCollection<long> userIds,
                                         IReadOnlyCollection<long> allotmentEventIds,
                                         IReadOnlyCollection<EthereumTransactionEntityType> ethereumTransactionEntityTypes,
                                         IReadOnlyCollection<EthereumTransactionType> ethereumTransactionTypes,
                                         IReadOnlyCollection<EthereumTransactionStatus> ethereumTransactionStatuses)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), "Offset should be greater or equal zero.");

            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count should be greater than 0.");

            Offset = offset;
            Count = count;
            UserIds = userIds ?? throw new ArgumentNullException(nameof(userIds));
            EntityIds = allotmentEventIds ?? throw new ArgumentNullException(nameof(allotmentEventIds));
            EntityTypes = ethereumTransactionEntityTypes ?? throw new ArgumentNullException(nameof(ethereumTransactionEntityTypes));
            TransactionTypes = ethereumTransactionTypes ?? throw new ArgumentNullException(nameof(ethereumTransactionTypes));
            Statuses = ethereumTransactionStatuses ?? throw new ArgumentNullException(nameof(ethereumTransactionStatuses));
        }

        public int Offset { get; }

        public int Count { get; }

        public IReadOnlyCollection<long> UserIds { get; }

        public IReadOnlyCollection<long> EntityIds { get; }

        public IReadOnlyCollection<EthereumTransactionEntityType> EntityTypes { get; }

        public IReadOnlyCollection<EthereumTransactionType> TransactionTypes { get; }

        public IReadOnlyCollection<EthereumTransactionStatus> Statuses { get; }
    }
}