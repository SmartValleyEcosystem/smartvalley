using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Domain.Contracts;
using SmartValley.Domain.Entities;
using SmartValley.Ethereum.Contracts.ExpertsRegistry.Dto;
using SmartValley.Ethereum.Contracts.SmartValley.Application.Contracts;

namespace SmartValley.Ethereum.Contracts.ExpertsRegistry
{
    public class ExpertsRegistryContractClient : IExpertsRegistryContractClient
    {
        private readonly EthereumContractClient _contractClient;

        private readonly string _contractAbi;

        public ExpertsRegistryContractClient(EthereumContractClient contractClient, ContractOptions contractOptions)
        {
            _contractClient = contractClient;
            _contractAbi = contractOptions.Abi;
        }

        public async Task<IReadOnlyCollection<int>> GetExpertAreasAsync(string address)
        {
            var dto = await _contractClient.CallFunctionDeserializingToObjectAsync<AreasDto>(address, _contractAbi, "getExpertAreas");
            return dto.Areas;
        }
    }
}