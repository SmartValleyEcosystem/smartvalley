using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Services
{
    public interface IEthereumTransactionService
    {
        Task<long> StartAsync(string hash, long userId, EthereumTransactionType type);

        Task CompleteAsync(string hash);

        Task FailAsync(string hash);
    }
}