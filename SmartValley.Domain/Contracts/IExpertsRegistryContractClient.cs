using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Contracts
{
    public interface IExpertsRegistryContractClient
    {
        Task<IReadOnlyCollection<int>> GetExpertAreasAsync(string address);
    }
}