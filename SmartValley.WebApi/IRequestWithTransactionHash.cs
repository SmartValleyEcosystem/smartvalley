namespace SmartValley.WebApi
{
    public interface IRequestWithTransactionHash
    {
        string TransactionHash { get; set; }
    }
}