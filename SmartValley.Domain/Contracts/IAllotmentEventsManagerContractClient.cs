using System.Threading.Tasks;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Contracts
{
    public interface IAllotmentEventsManagerContractClient
    {
        Task<Address> GetAllotmentEventContractAddressAsync(long eventId);

        Task<bool> IsDeletedAsync(long eventId);
    }
}