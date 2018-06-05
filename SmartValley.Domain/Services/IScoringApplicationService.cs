using System.Threading.Tasks;

namespace SmartValley.Domain.Services
{
    public interface IScoringApplicationService
    {
        Task SetScoringTransactionAsync(long projectId, string transactionHash);
    }
}