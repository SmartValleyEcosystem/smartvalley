using System.Threading.Tasks;

namespace SmartValley.WebApi.Contract
{
    public class EtherManagerContractServiceStub : IEtherManagerContractService
    {
        public Task<bool> HasReceivedEtherAsync(string address)
        {
            return Task.FromResult(false);
        }

        public Task<double> GetEtherBalanceAsync(string address)
        {
            return Task.FromResult(1.5);
        }

        public Task SendEtherToAsync(string address)
        {
            return  Task.Delay(5000);
        }
    }
}