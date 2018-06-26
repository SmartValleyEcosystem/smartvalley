using System;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.EtheriumTransactions.Response
{
    public class EthereumTransactionResponse
    {
        private EthereumTransactionResponse()
        {
        }

        public long Id { get; private set; }

        public long UserId { get; private set; }

        public string Hash { get; private set; }

        public long EntityId { get; private set; }

        public EthereumTransactionEntityType EntityType { get; private set; }

        public EthereumTransactionType TransactionType { get; private set; }

        public EthereumTransactionStatus Status { get; private set; }

        public DateTimeOffset Created { get; private set; }

        public static EthereumTransactionResponse Create(EthereumTransaction transaction)
        {
            return new EthereumTransactionResponse
                   {
                       Id = transaction.Id,
                       UserId = transaction.UserId,
                       Hash = transaction.Hash,
                       EntityId = transaction.EntityId,
                       EntityType = transaction.EntityType,
                       TransactionType = transaction.TransactionType,
                       Status = transaction.Status,
                       Created = transaction.Created
                   };
        }
    }
}