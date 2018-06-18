namespace SmartValley.Messages.Commands
{
    public class UpdateExpertAreas
    {
        public string ExpertAddress { get; set; }

        public string TransactionHash { get; set; }

        public long UserId { get; set; }
    }
}