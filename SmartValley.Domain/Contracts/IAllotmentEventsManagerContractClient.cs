using System.Threading.Tasks;

namespace SmartValley.Domain.Contracts
{
    public interface IAllotmentEventsManagerContractClient
    {
        Task<string> GetAllotmentEventContractAddressAsync(long eventId);
    }
}