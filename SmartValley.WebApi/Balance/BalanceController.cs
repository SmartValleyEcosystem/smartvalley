using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Application;
using SmartValley.Application.Contracts;
using SmartValley.WebApi.Balance.Responses;

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
        public async Task<BalanceResponse> GetAsync()
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
        public async Task<ReceiveEtherResponse> ReceiveEtherAsync()
        {
            var transactionHash = await _etherManagerContractClient.SendEtherToAsync(User.Identity.Name);
            return new ReceiveEtherResponse{TransactionHash = transactionHash};
        }
    }
}