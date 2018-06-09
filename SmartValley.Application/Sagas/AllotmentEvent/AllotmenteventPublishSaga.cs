using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using SmartValley.Domain;
using SmartValley.Domain.Services;
using SmartValley.Messages.Commands;
using SmartValley.Messages.Events;

namespace SmartValley.Application.Sagas.AllotmentEvent
{
    public class AllotmentEventPublishSaga : SqlSaga<AllotmentEventPublishSagaData>,
        IAmStartedByMessages<PublishAllotmentEventCommand>,
        IHandleMessages<TransactionCompleted>,
        IHandleMessages<TransactionFailed>
    {
        private readonly IAllotmentEventService _allotmentEventService;

        public AllotmentEventPublishSaga(IAllotmentEventService allotmentEventService)
        {
            _allotmentEventService = allotmentEventService;
        }

        protected override string CorrelationPropertyName => nameof(PublishAllotmentEventCommand.TransactionHash);

        protected override void ConfigureMapping(IMessagePropertyMapper mapper)
        {
            mapper.ConfigureMapping<PublishAllotmentEventCommand>(x => x.TransactionHash);

            mapper.ConfigureMapping<TransactionCompleted>(m => m.TransactionHash);
            mapper.ConfigureMapping<TransactionFailed>(m => m.TransactionHash);
        }

        public async Task Handle(PublishAllotmentEventCommand message, IMessageHandlerContext context)
        {
            Data.AllotmentEventId = message.AllotmentEventId;

            await _allotmentEventService.HandleSuccessPublishingTransaction(message.AllotmentEventId, AllotmentEventStatus.Publishing);
            await context.SendLocal(new WaitForTransaction {TransactionHash = message.TransactionHash});
        }

        public async Task Handle(TransactionCompleted message, IMessageHandlerContext context)
        {
            await _allotmentEventService.HandleSuccessPublishingTransaction(Data.AllotmentEventId, AllotmentEventStatus.Published);

            MarkAsComplete();
        }

        public async Task Handle(TransactionFailed message, IMessageHandlerContext context)
        {
            await _allotmentEventService.HandleSuccessPublishingTransaction(Data.AllotmentEventId, AllotmentEventStatus.Created);

            MarkAsComplete();
        }
    }
}
