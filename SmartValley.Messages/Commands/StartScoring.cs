namespace SmartValley.Messages.Commands
{
    public class StartScoring
    {
        public string TransactionHash { get; set; }

        public long ProjectId { get; set; }

        public long UserId { get; set; }
    }
}