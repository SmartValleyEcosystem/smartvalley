using System.Threading.Tasks;

namespace SmartValley.Domain.Services
{
    public interface IEthereumTransactionService
    {
        Task CompleteAsync(string hash);

        Task FailAsync(string hash);
    }
}