using System.Threading.Tasks;
using Nethereum.ABI.Encoders;

namespace SmartValley.Application.Contracts
{
    public class ProjectManagerContractClient : EthereumContractClient, IProjectManagerContractClient
    {
        public ProjectManagerContractClient(NethereumOptions nethereumOptions)
            : base(nethereumOptions.RpcAddress, nethereumOptions.ProjectManagerContract)
        {
        }

        public async Task<string> AddProjectAsync(string projectId, string signedTransactionData)
        {
            await SendRawTransactionAsync(signedTransactionData);

            var encodedProjectId = new Bytes32TypeEncoder().Encode(projectId);
            return await GetFunction("projectsMap").CallAsync<string>(encodedProjectId);
        }
    }
}