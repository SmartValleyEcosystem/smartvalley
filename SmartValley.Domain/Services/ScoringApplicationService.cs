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
        private readonly IEthereumTransactionService _ethereumTransactionService;

        public ScoringApplicationService(
            IScoringApplicationRepository scoringApplicationRepository,
            IEthereumTransactionService ethereumTransactionService)
        {
            _scoringApplicationRepository = scoringApplicationRepository;
            _ethereumTransactionService = ethereumTransactionService;
        }

        public async Task SetScoringTransactionAsync(long projectId, string transactionHash, long userId)
        {
            var application = await _scoringApplicationRepository.GetByProjectIdAsync(projectId) ?? throw new AppErrorException(ErrorCode.ScoringApplicationNotFound);
            var transactionId = await _ethereumTransactionService.StartAsync(transactionHash, userId, EthereumTransactionType.StartScoring);

            application.ScoringStartTransactionId = transactionId;

            await _scoringApplicationRepository.SaveChangesAsync();
        }
    }
}