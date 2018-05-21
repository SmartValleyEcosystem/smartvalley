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

        [HttpGet("scoringExpertsManager")]
        public ContractResponse GetScoringExpertsManagerContract()
        {
            var contractOptions = _nethereumOptions.ScoringExpertsManagerContract;
            return new ContractResponse
                   {
                       Address = contractOptions.Address,
                       Abi = contractOptions.Abi,
                   };
        }

        [Route("scoringManager")]
        [HttpGet]
        public ContractResponse GetScoringManagerContract()
        {
            var contractOptions = _nethereumOptions.ScoringsRegistryContract;
            return new ContractResponse
                   {
                       Address = contractOptions.Address,
                       Abi = contractOptions.Abi,
                   };
        }

        [Route("scoring")]
        [HttpGet]
        public ContractResponse GetScoringContract()
        {
            var contractOptions = _nethereumOptions.ScoringContract;
            return new ContractResponse
                   {
                       Abi = contractOptions.Abi
                   };
        }

        [Route("expertsRegistry")]
        [HttpGet]
        public ContractResponse GetExpertsRegistryContract()
        {
            var contractOptions = _nethereumOptions.ExpertsRegistryContract;
            return new ContractResponse
                   {
                       Address = contractOptions.Address,
                       Abi = contractOptions.Abi,
                   };
        }

        [Route("adminRegistry")]
        [HttpGet]
        public ContractResponse GetAdminRegistryContract()
        {
            var contractOptions = _nethereumOptions.AdminRegistryContract;
            return new ContractResponse
                   {
                       Address = contractOptions.Address,
                       Abi = contractOptions.Abi,
                   };
        }
    }
}