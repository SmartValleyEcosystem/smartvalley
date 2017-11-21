using System;
using System.Threading.Tasks;
using Nethereum.Hex.HexConvertors.Extensions;

namespace SmartValley.Application.Contracts
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ProjectManagerContractClient : IProjectManagerContractClient
    {
        private readonly EthereumContractClient _contractClient;

        private readonly string _contractAddress;
        private readonly string _contractAbi;

        public ProjectManagerContractClient(EthereumContractClient contractClient, ContractOptions contractOptions)
        {
            _contractClient = contractClient;
            _contractAddress = contractOptions.Address;
            _contractAbi = contractOptions.Abi;
        }

        public async Task<string> GetProjectAddressAsync(string projectId, string transactionHash)
        {
            await _contractClient.WaitForConfirmationAsync(transactionHash);

            var projectAddress = await _contractClient.CallFunctionAsync<string>(_contractAddress, _contractAbi, "projectsMap", projectId.Replace("-", ""));

            if (IsAddressEmpty(projectAddress))
                throw new InvalidOperationException("Project id was not found in contract.");

            return projectAddress;
        }

        private static bool IsAddressEmpty(string projectAddress) => string.IsNullOrWhiteSpace(projectAddress.RemoveHexPrefix().Replace("0", ""));
    }
}
