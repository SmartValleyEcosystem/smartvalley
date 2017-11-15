using System.Threading.Tasks;

namespace SmartValley.Application.Contracts
{
    public interface IEtherManagerContractService
    {
        Task<bool> HasReceivedEtherAsync(string address);

        Task SendEtherToAsync(string address);
    }
}