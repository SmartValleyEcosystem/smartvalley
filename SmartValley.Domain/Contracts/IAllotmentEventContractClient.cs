using System.Threading.Tasks;

namespace SmartValley.Domain.Contracts
{
    public interface IAllotmentEventContractClient
    {
        Task<AllotmentEventInfo> GetInfoAsync(string contractAddress);
    }
}