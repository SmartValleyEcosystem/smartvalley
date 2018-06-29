namespace SmartValley.Ethereum
{
    public class TransactionInfo
    {
        private TransactionInfo(TransactionStatus status, long? gasUsed = null)
        {
            Status = status;
            GasUsed = gasUsed;
        }

        public TransactionStatus Status { get; }

        public long? GasUsed { get; }

        public static TransactionInfo NotMined() => new TransactionInfo(TransactionStatus.NotMined);

        public static TransactionInfo NotConfirmed() => new TransactionInfo(TransactionStatus.NotConfirmed);

        public static TransactionInfo Completed(long gasUsed) => new TransactionInfo(TransactionStatus.Completed, gasUsed);

        public static TransactionInfo Failed(long gasUsed) => new TransactionInfo(TransactionStatus.Failed, gasUsed);
    }
}