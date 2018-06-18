using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IEthereumTransactionRepository
    {
        Task<EthereumTransaction> GetByHashAsync(string hash);

        Task SaveChangesAsync();

        void Add(EthereumTransaction ethereumTransaction);
    }
}