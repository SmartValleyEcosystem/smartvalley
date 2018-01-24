using Microsoft.AspNetCore.Mvc;
using SmartValley.Application.Contracts;
using SmartValley.Application.Contracts.Options;
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

        [Route("scoringManager")]
        public ContractResponse GetScoringManagerContract()
        {
            var contractOptions = _nethereumOptions.ScoringManagerContract;
            return new ContractResponse
                   {
                       Address = contractOptions.Address,
                       Abi = contractOptions.Abi,
                   };
        }

        [Route("votingManager")]
        public ContractResponse GetVotingManagerContract()
        {
            var contractOptions = _nethereumOptions.VotingManagerContract;
            return new ContractResponse
                   {
                       Address = contractOptions.Address,
                       Abi = contractOptions.Abi,
                   };
        }

        [Route("voting")]
        public ContractResponse GetVotingContract()
        {
            var contractOptions = _nethereumOptions.VotingSprintContract;
            return new ContractResponse
                   {
                       Abi = contractOptions.Abi
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