using System.Threading.Tasks;

namespace SmartValley.Application.Contracts
{
    public interface IProjectManagerContractClient
    {
        Task<string> AddProjectAsync(string author, string applicationHash, string name);
    }
}