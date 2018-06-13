using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using SmartValley.Domain.Services;
using SmartValley.Messages.Commands;
using SmartValley.Messages.Events;

namespace SmartValley.Application.Sagas.AllotmentEvent
{
    public class AllotmentEventPublishSaga : SqlSaga<AllotmentEventPublishSagaData>,
        IAmStartedByMessages<PublishAllotment>,
        IHandleMessages<TransactionCompleted>,
        IHandleMessages<TransactionFailed>
    {
        private readonly IAllotmentEventService _allotmentEventService;

        public AllotmentEventPublishSaga(IAllotmentEventService allotmentEventService)
        {
            _allotmentEventService = allotmentEventService;
        }

        protected override string CorrelationPropertyName => nameof(PublishAllotment.TransactionHash);

        protected override void ConfigureMapping(IMessagePropertyMapper mapper)
        {
            mapper.ConfigureMapping<PublishAllotment>(x => x.TransactionHash);

            mapper.ConfigureMapping<TransactionCompleted>(m => m.TransactionHash);
            mapper.ConfigureMapping<TransactionFailed>(m => m.TransactionHash);
        }

        public async Task Handle(PublishAllotment command, IMessageHandlerContext context)
        {
            Data.AllotmentEventId = command.AllotmentEventId;
            Data.UserId = command.UserId;
            Data.TransactionHash = command.TransactionHash;

            await _allotmentEventService.HandlePendingPublishingTransactionAsync(command.AllotmentEventId, command.UserId, command.TransactionHash);
            await context.SendLocal(new WaitForTransaction {TransactionHash = command.TransactionHash});
        }

        public async Task Handle(TransactionCompleted message, IMessageHandlerContext context)
        {
            await _allotmentEventService.HandleSuccessPublishingTransactionAsync(Data.AllotmentEventId);
            MarkAsComplete();
        }

        public async Task Handle(TransactionFailed message, IMessageHandlerContext context)
        {
            await _allotmentEventService.HandleFailedPublishingTransactionAsync(Data.AllotmentEventId);

            MarkAsComplete();
        }
    }
}
