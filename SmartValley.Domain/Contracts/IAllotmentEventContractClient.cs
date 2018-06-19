using System.Threading.Tasks;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Contracts
{
    public interface IAllotmentEventContractClient
    {
        Task<AllotmentEventInfo> GetInfoAsync(string contractAddress);
    }
}