using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Application;
using SmartValley.Application.Contracts;

namespace SmartValley.WebApi.Balance
{
    [Route("api/balance/")]
    [Authorize]
    public class BalanceController : Controller
    {
        private readonly IEtherManagerContractClient _etherManagerContractClient;
        private readonly EthereumClient _ethereumClient;

        public BalanceController(IEtherManagerContractClient etherManagerContractClient, EthereumClient ethereumClient)
        {
            _etherManagerContractClient = etherManagerContractClient;
            _ethereumClient = ethereumClient;
        }

        [HttpGet]
        public async Task<BalanceResponse> Get()
        {
            var wasEtherReceived = await _etherManagerContractClient.HasReceivedEtherAsync(User.Identity.Name);
            var balance = await _ethereumClient.GetBalanceAsync(User.Identity.Name);

            return new BalanceResponse
                   {
                       WasEtherReceived = wasEtherReceived,
                       Balance = balance
                   };
        }

        [HttpPost]
        public Task Post() => _etherManagerContractClient.SendEtherToAsync(User.Identity.Name);
    }
}