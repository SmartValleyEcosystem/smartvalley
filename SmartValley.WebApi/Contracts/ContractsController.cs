using Microsoft.AspNetCore.Mvc;
using SmartValley.Ethereum;
using SmartValley.WebApi.Contracts.Responses;

namespace SmartValley.WebApi.Contracts
{
    [Route("api/contracts")]
    public class ContractsController : Controller
    {
        private readonly NethereumOptions _nethereumOptions;

        public ContractsController(NethereumOptions nethereumOptions)
        {
            _nethereumOptions = nethereumOptions;
        }

        [HttpGet("allotmentEventsManager")]
        public ContractResponse GetAllotmentEventsManagerContract()
            => ContractResponse.FromOptions(_nethereumOptions.AllotmentEventsManagerContract);

        [HttpGet("allotmentEvents")]
        public ContractResponse GetAllotmentEventsContract()
            => ContractResponse.FromOptions(_nethereumOptions.AllotmentEventContract);

        [HttpGet("erc223")]
        public ContractResponse GetERC223Contract()
            => ContractResponse.FromOptions(_nethereumOptions.ERC223Contract);

        [HttpGet("smartValleyToken")]
        public ContractResponse GetSmartValleyTokenContract()
            => ContractResponse.FromOptions(_nethereumOptions.SmartValleyTokenContract);

        [HttpGet("scoringOffersManager")]
        public ContractResponse GetScoringOffersManagerContract()
            => ContractResponse.FromOptions(_nethereumOptions.ScoringOffersManagerContract);

        [HttpGet("scoringManager")]
        public ContractResponse GetScoringManagerContract()
            => ContractResponse.FromOptions(_nethereumOptions.ScoringManagerContract);

        [HttpGet("privateScoringManager")]
        public ContractResponse GetPrivateScoringManagerContract()
            => ContractResponse.FromOptions(_nethereumOptions.PrivateScoringManagerContract);

        [HttpGet("scoringsRegistry")]
        public ContractResponse GetScoringsRegistryContract()
            => ContractResponse.FromOptions(_nethereumOptions.ScoringsRegistryContract);

        [HttpGet("scoring")]
        public ContractResponse GetScoringContract()
            => ContractResponse.FromOptions(_nethereumOptions.ScoringContract);

        [HttpGet("expertsRegistry")]
        public ContractResponse GetExpertsRegistryContract()
            => ContractResponse.FromOptions(_nethereumOptions.ExpertsRegistryContract);

        [HttpGet("adminRegistry")]
        public ContractResponse GetAdminRegistryContract()
            => ContractResponse.FromOptions(_nethereumOptions.AdminRegistryContract);

        [HttpGet("scoringParametersProvider")]
        public ContractResponse GetScoringParametersProviderContract()
            => ContractResponse.FromOptions(_nethereumOptions.ScoringParametersProviderContract);

        [HttpGet("minter")]
        public ContractResponse GetMinterContract()
            => ContractResponse.FromOptions(_nethereumOptions.MinterContract);
    }
}