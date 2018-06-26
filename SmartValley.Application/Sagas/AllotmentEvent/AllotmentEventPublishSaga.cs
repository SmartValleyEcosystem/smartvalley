using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Services;
using SmartValley.Messages.Commands;
using SmartValley.Messages.Events;

namespace SmartValley.Application.Sagas.AllotmentEvent
{
    public class AllotmentEventPublishSaga : SqlSaga<AllotmentEventPublishSagaData>,
        IAmStartedByMessages<PublishAllotmentEvent>,
        IHandleMessages<TransactionCompleted>,
        IHandleMessages<TransactionFailed>
    {
        private readonly IAllotmentEventService _allotmentEventService;
        private readonly IEthereumTransactionService _ethereumTransactionService;

        public AllotmentEventPublishSaga(IAllotmentEventService allotmentEventService, IEthereumTransactionService ethereumTransactionService)
        {
            _allotmentEventService = allotmentEventService;
            _ethereumTransactionService = ethereumTransactionService;
        }

        protected override string CorrelationPropertyName => nameof(AllotmentEventPublishSagaData.TransactionHash);

        protected override void ConfigureMapping(IMessagePropertyMapper mapper)
        {
            mapper.ConfigureMapping<PublishAllotmentEvent>(x => x.TransactionHash);

            mapper.ConfigureMapping<TransactionCompleted>(m => m.TransactionHash);
            mapper.ConfigureMapping<TransactionFailed>(m => m.TransactionHash);
        }

        public async Task Handle(PublishAllotmentEvent command, IMessageHandlerContext context)
        {
            Data.AllotmentEventId = command.AllotmentEventId;
            Data.UserId = command.UserId;
            Data.TransactionHash = command.TransactionHash;

            await _allotmentEventService.SetUpdatingStateAsync(command.AllotmentEventId, true);
            await _ethereumTransactionService.StartAsync(command.TransactionHash, command.UserId, EthereumTransactionEntityType.AllotmentEvent, command.AllotmentEventId, EthereumTransactionType.PublishAllotmentEvent);
            await context.SendLocal(new WaitForTransaction {TransactionHash = command.TransactionHash});
        }

        public async Task Handle(TransactionCompleted message, IMessageHandlerContext context)
        {
            await _allotmentEventService.SetUpdatingStateAsync(Data.AllotmentEventId, false);
            await _allotmentEventService.PublishAsync(Data.AllotmentEventId);
            MarkAsComplete();
        }

        public async Task Handle(TransactionFailed message, IMessageHandlerContext context)
        {
            await _allotmentEventService.SetUpdatingStateAsync(Data.AllotmentEventId, false);
            MarkAsComplete();
        }
    }
}
