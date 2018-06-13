using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IEthereumTransactionRepository
    {
        Task<EthereumTransaction> GetByHashAsync(string hash);

        Task SaveChangesAsync();

        Task<IReadOnlyCollection<EthereumTransaction>> GetByAllotmentEventIdAsync(long allotmentEventId);

        void Add(EthereumTransaction ethereumTransaction);
    }
}