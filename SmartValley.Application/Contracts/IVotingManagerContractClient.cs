using System.Threading.Tasks;

namespace SmartValley.Application.Contracts
{
    public interface IVotingManagerContractClient
    {
        Task<string> GetLastSprintAddressAsync();
    }
}
