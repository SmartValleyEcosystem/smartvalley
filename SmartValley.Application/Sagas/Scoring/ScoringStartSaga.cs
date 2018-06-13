using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using SmartValley.Domain.Services;
using SmartValley.Messages.Commands;
using SmartValley.Messages.Events;

namespace SmartValley.Application.Sagas.Scoring
{
    // ReSharper disable once UnusedMember.Global
    public class ScoringStartSaga :
        SqlSaga<ScoringStartSagaData>,
        IAmStartedByMessages<StartScoring>,
        IHandleMessages<TransactionCompleted>,
        IHandleMessages<TransactionFailed>
    {
        private readonly IScoringService _scoringService;
        private readonly IScoringApplicationService _scoringApplicationService;

        public ScoringStartSaga(IScoringService scoringService, IScoringApplicationService scoringApplicationService)
        {
            _scoringService = scoringService;
            _scoringApplicationService = scoringApplicationService;
        }

        protected override string CorrelationPropertyName => nameof(ScoringStartSagaData.TransactionHash);

        public async Task Handle(StartScoring message, IMessageHandlerContext context)
        {
            Data.ProjectId = message.ProjectId;
            Data.TransactionHash = message.TransactionHash;

            await _scoringApplicationService.SetScoringTransactionAsync(message.ProjectId, message.TransactionHash);

            await context.SendLocal(new WaitForTransaction {TransactionHash = message.TransactionHash});
        }

        public async Task Handle(TransactionCompleted message, IMessageHandlerContext context)
        {
            var scoringId = await _scoringService.StartAsync(Data.ProjectId);

            await SendOffersAsync(context, scoringId);

            MarkAsComplete();
        }

        public Task Handle(TransactionFailed message, IMessageHandlerContext context)
        {
            MarkAsComplete();
            return Task.CompletedTask;
        }

        protected override void ConfigureMapping(IMessagePropertyMapper mapper)
        {
            mapper.ConfigureMapping<StartScoring>(m => m.TransactionHash);

            mapper.ConfigureMapping<TransactionCompleted>(m => m.TransactionHash);
            mapper.ConfigureMapping<TransactionFailed>(m => m.TransactionHash);
        }

        private async Task SendOffersAsync(IMessageHandlerContext context, long scoringId)
        {
            var scoring = await _scoringService.GetByIdAsync(scoringId);

            foreach (var expertId in scoring.ScoringOffers.Select(o => o.ExpertId).Distinct())
                await context.SendLocal(new SendScoringOfferNotification {ExpertId = expertId});
        }
    }
}