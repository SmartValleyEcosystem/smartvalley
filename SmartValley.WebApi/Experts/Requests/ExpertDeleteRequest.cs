namespace SmartValley.WebApi.Experts.Requests
{
    public class ExpertDeleteRequest : IRequestWithTransactionHash
    {
        public string TransactionHash { get; set; }

        public string Address { get; set; }
    }
}