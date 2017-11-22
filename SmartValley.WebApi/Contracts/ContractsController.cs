using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Application.Contracts;

namespace SmartValley.WebApi.Contracts
{
    [Route("api/contracts")]
    [Authorize]
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
            var projectManagerContractOptions = _nethereumOptions.ProjectManagerContract;
            return new ContractResponse
                   {
                       Address = projectManagerContractOptions.Address,
                       Abi = projectManagerContractOptions.Abi,
                   };
        }
    }
}