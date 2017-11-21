using System.Threading.Tasks;

namespace SmartValley.Application.Contracts
{
    public interface IProjectManagerContractClient
    {
        Task<string> GetProjectAddressAsync(string projectId, string transactionHash);
    }
}