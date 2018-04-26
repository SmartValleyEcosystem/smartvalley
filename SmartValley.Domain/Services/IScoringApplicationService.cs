using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Services
{
    public interface IScoringApplicationService
    {
        Task SetScoringTransactionAsync(long projectId, string transactionHash);
    }
}