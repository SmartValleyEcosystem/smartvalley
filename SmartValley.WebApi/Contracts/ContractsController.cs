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
        {
            var contractOptions = _nethereumOptions.AllotmentEventsManagerContract;
            return new ContractResponse
                   {
                       Address = contractOptions.Address,
                       Abi = contractOptions.Abi,
                   };
        }

        [HttpGet("erc223")]
        public ContractResponse GetERC223Contract()
        {
            var contractOptions = _nethereumOptions.ERC223Contract;
            return new ContractResponse
                   {
                       Abi = contractOptions.Abi
                   };
        }

        [HttpGet("smartValleyToken")]
        public ContractResponse GetSmartValleyTokenContract()
        {
            var contractOptions = _nethereumOptions.SmartValleyTokenContract;
            return new ContractResponse
                   {
                       Address = contractOptions.Address,
                       Abi = contractOptions.Abi
                   };
        }

        [HttpGet("scoringOffersManager")]
        public ContractResponse GetScoringOffersManagerContract()
        {
            var contractOptions = _nethereumOptions.ScoringOffersManagerContract;
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
            var contractOptions = _nethereumOptions.ScoringManagerContract;
            return new ContractResponse
                   {
                       Address = contractOptions.Address,
                       Abi = contractOptions.Abi,
                   };
        }

        [Route("privateScoringManager")]
        [HttpGet]
        public ContractResponse GetPrivateScoringManagerContract()
        {
            var contractOptions = _nethereumOptions.PrivateScoringManagerContract;
            return new ContractResponse
                   {
                       Address = contractOptions.Address,
                       Abi = contractOptions.Abi,
                   };
        }

        [Route("scoringsRegistry")]
        [HttpGet]
        public ContractResponse GetScoringsRegistryContract()
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

        [Route("scoringParametersProvider")]
        [HttpGet]
        public ContractResponse GetScoringParametersProviderContract()
        {
            var contractOptions = _nethereumOptions.ScoringParametersProviderContract;
            return new ContractResponse
                   {
                       Address = contractOptions.Address,
                       Abi = contractOptions.Abi,
                   };
        }
    }
}