using System.Threading.Tasks;

namespace SmartValley.Application.Contracts
{
    public class ProjectManagerContractClient : EthereumContractClient, IProjectManagerContractClient
    {
        public ProjectManagerContractClient(NethereumOptions nethereumOptions)
            : base(nethereumOptions.RpcAddress, nethereumOptions.ProjectManagerContract)
        {
        }

        public Task<string> AddProjectAsync(string author, string applicationHash, string name)
            => SignAndSendTransactionAsync<string>("addProject", author, applicationHash, name);
    }
}