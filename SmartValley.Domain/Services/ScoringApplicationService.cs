using System.Threading.Tasks;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Domain.Services
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ScoringApplicationService : IScoringApplicationService
    {
        private readonly IScoringApplicationRepository _scoringApplicationRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IClock _clock;

        public ScoringApplicationService(IScoringApplicationRepository scoringApplicationRepository, IProjectRepository projectRepository, IClock clock)
        {
            _scoringApplicationRepository = scoringApplicationRepository;
            _clock = clock;
            _projectRepository = projectRepository;
        }

        public async Task SetScoringTransactionAsync(long projectId, string transactionHash)
        {
            var application = await _scoringApplicationRepository.GetByProjectIdAsync(projectId) ?? throw new AppErrorException(ErrorCode.ScoringApplicationNotFound);
            var project = await _projectRepository.GetByIdAsync(projectId);
            var transaction = new EthereumTransaction(
                project.AuthorId,
                transactionHash,
                EthereumTransactionType.StartScoring,
                EthereumTransactionStatus.InProgress,
                _clock.UtcNow);

            application.SetScoringStartTransaction(transaction);
            await _scoringApplicationRepository.SaveChangesAsync();
        }
    }
}