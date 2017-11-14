using System.Threading.Tasks;

namespace SmartValley.WebApi.Contract
{
    public interface IEtherManagerContractService
    {
        Task<bool> HasReceivedEtherAsync(string address);

        Task<double> GetEtherBalanceAsync(string address);

        Task SendEtherToAsync(string address);
    }
}