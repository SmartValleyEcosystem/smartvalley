using System.Threading.Tasks;

namespace SmartValley.Application.Contracts
{
    public class ProjectManagerContractService : EthereumContractClient, IProjectManagerContractService
    {
        public ProjectManagerContractService(NethereumOptions nethereumOptions)
            : base(nethereumOptions.RpcAddress, nethereumOptions.ProjectManagerContract)
        {
        }

        public Task<string> AddProjectAsync(string author, string applicationHash, string name)
            => SignAndSendTransactionAsync<string>("addProject", author, applicationHash, name);
    }
}