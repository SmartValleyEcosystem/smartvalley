using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.Contract;

namespace SmartValley.WebApi.Balance
{
    [Route("api/balance/")]
    [Authorize]
    public class BalanceController : Controller
    {
        private readonly IEtherManagerContractService _etherManagerContractService;

        public BalanceController(IEtherManagerContractService etherManagerContractService)
        {
            _etherManagerContractService = etherManagerContractService;
        }

        [HttpGet]
        public async Task<BalanceResponse> Get()
        {
            var wasEtherReceived = await _etherManagerContractService.HasReceivedEtherAsync(User.Identity.Name);
            var balance = await _etherManagerContractService.GetEtherBalanceAsync(User.Identity.Name);

            return new BalanceResponse
                   {
                       WasEtherReceived = wasEtherReceived,
                       Balance = balance
                   };
        }

        [HttpPost]
        public Task Post() => _etherManagerContractService.SendEtherToAsync(User.Identity.Name);
    }
}