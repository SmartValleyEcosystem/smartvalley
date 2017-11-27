using System.Threading.Tasks;

namespace SmartValley.Application.Contracts
{
    public interface IProjectManagerContractClient
    {
        Task<string> GetProjectAddressAsync(string projectIdString, string transactionHash);
    }
}