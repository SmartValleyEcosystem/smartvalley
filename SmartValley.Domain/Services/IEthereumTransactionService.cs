using System.Threading.Tasks;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Services
{
    public interface IEthereumTransactionService
    {
        Task<long> StartAsync(string hash, long userId, EthereumTransactionEntityType entityType, long entityId, EthereumTransactionType transactionType);

        Task CompleteAsync(string hash);

        Task FailAsync(string hash);

        Task<PagingCollection<EthereumTransaction>> GetAsync(EtheriumTransactionsQuery query);
    }
}