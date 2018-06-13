using System;

namespace SmartValley.Domain.Entities
{
    public class EthereumTransaction
    {
        public EthereumTransaction(
            long userId,
            string hash,
            EthereumTransactionType type,
            EthereumTransactionStatus status,
            DateTimeOffset created,
            long? allotmentEventId = null)
        {
            UserId = userId;
            Hash = hash;
            Type = type;
            Status = status;
            Created = created;
            AllotmentEventId = allotmentEventId;
        }

        // ReSharper disable once UnusedMember.Local
        private EthereumTransaction()
        {
        }

        public long Id { get; set; }

        public long UserId { get; set; }

        public long? AllotmentEventId { get; set; }

        public string Hash { get; set; }

        public EthereumTransactionType Type { get; set; }

        public EthereumTransactionStatus Status { get; set; }

        public DateTimeOffset Created { get; set; }

        public User User { get; set; }

        public AllotmentEvent AllotmentEvent { get; set; }

        public void Complete()
            => SetStatus(EthereumTransactionStatus.Completed);

        public void Fail()
            => SetStatus(EthereumTransactionStatus.Failed);

        private void SetStatus(EthereumTransactionStatus status)
        {
            if (Status != EthereumTransactionStatus.InProgress)
                throw new InvalidOperationException($"Status of completed transaction '{Hash}' cannot be changed");

            Status = status;
        }
    }
}