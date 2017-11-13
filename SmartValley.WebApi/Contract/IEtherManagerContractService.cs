using System.Threading.Tasks;

namespace SmartValley.WebApi.Contract
{
    public interface IEtherManagerContractService
    {
        Task<bool> HadReceivedEtherAsync(string address);

        Task<double> GetEtherBalanceAsync(string address);
        Task SendEtherTo(string address);
    }
}