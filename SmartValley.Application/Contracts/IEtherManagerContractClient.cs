using System.Threading.Tasks;

namespace SmartValley.Application.Contracts
{
    public interface IEtherManagerContractClient
    {
        Task<bool> HasReceivedEtherAsync(string address);

        Task SendEtherToAsync(string address);
    }
}