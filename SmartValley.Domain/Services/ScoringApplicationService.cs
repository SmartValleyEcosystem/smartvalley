using System.Threading.Tasks;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Domain.Services
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ScoringApplicationService : IScoringApplicationService
    {
        private readonly IScoringApplicationRepository _repository;
        private readonly IClock _clock;

        public ScoringApplicationService(IScoringApplicationRepository repository, IClock clock)
        {
            _repository = repository;
            _clock = clock;
        }

        public async Task SetScoringTransactionAsync(long projectId, string transactionHash)
        {
            var application = await _repository.GetByProjectIdAsync(projectId) ?? throw new AppErrorException(ErrorCode.ScoringApplicationNotFound);
            application.SetScoringStartTransaction(transactionHash, _clock.UtcNow);

            await _repository.SaveChangesAsync();
        }
    }
}