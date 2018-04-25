using System.Threading.Tasks;

namespace SmartValley.Domain.Contracts
{
    public interface IEtherManagerContractClient
    {
        Task<bool> HasReceivedEtherAsync(string address);

        Task<string> SendEtherToAsync(string address);
    }
}