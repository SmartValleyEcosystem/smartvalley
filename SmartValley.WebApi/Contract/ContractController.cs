using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Application.Contracts;

namespace SmartValley.WebApi.Contract
{
    [Route("api/contract")]
    [Authorize]
    public class ContractController : Controller
    {
        private readonly NethereumOptions _nethereumOptions;

        public ContractController(NethereumOptions nethereumOptions)
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