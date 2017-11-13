using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.Contract;

namespace SmartValley.WebApi.User
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
            return new BalanceResponse
                   {
                       HadReceiviedEther = await _etherManagerContractService.HadReceivedEtherAsync(User.Identity.Name),
                       Balance = await _etherManagerContractService.GetEtherBalanceAsync(User.Identity.Name)
                   };
        }

        [HttpPost]
        public async Task Post()
        {
            await _etherManagerContractService.SendEtherTo(User.Identity.Name);
        }
    }
}