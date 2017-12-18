using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Application.Contracts;

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

        [Route("projectManager")]
        public ContractResponse GetProjectManagerContract()
        {
            var contractOptions = _nethereumOptions.ProjectManagerContract;
            return new ContractResponse
                   {
                       Address = contractOptions.Address,
                       Abi = contractOptions.Abi,
                   };
        }

        [Route("scoring")]
        public ContractResponse GetScoringContract()
        {
            var contractOptions = _nethereumOptions.ScoringContract;
            return new ContractResponse
                   {
                       Abi = contractOptions.Abi,
                       Address = contractOptions.Address
                   };
        }

        [Route("token")]
        public ContractResponse GetTokenContract()
        {
            var contractOptions = _nethereumOptions.TokenContract;
            return new ContractResponse
                   {
                       Address = contractOptions.Address,
                       Abi = contractOptions.Abi,
                   };
        }

        [Route("minter")]
        public ContractResponse GetMinterContract()
        {
            var contractOptions = _nethereumOptions.MinterContract;
            return new ContractResponse
                   {
                       Address = contractOptions.Address,
                       Abi = contractOptions.Abi,
                   };
        }
    }
}