using System.Threading.Tasks;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IEthereumTransactionRepository
    {
        Task<PagingCollection<EthereumTransaction>> GetAsync(EtheriumTransactionsQuery query);

        Task<EthereumTransaction> GetByHashAsync(string hash);

        void Add(EthereumTransaction ethereumTransaction);

        Task SaveChangesAsync();
    }
}