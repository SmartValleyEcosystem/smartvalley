using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Services;
using SmartValley.Messages.Events;
using SmartValley.Messages.Commands;

namespace SmartValley.Application.Sagas.AllotmentEvent
{
    public class AllotmentEventStartSaga : SqlSaga<AllotmentEventStartSagaData>,
        IAmStartedByMessages<StartAllotmentEvent>,
        IHandleMessages<TransactionCompleted>,
        IHandleMessages<TransactionFailed>
    {
        private readonly IAllotmentEventService _allotmentEventService;
        private readonly IEthereumTransactionService _ethereumTransactionService;

        public AllotmentEventStartSaga(IAllotmentEventService allotmentEventService, IEthereumTransactionService ethereumTransactionService)
        {
            _allotmentEventService = allotmentEventService;
            _ethereumTransactionService = ethereumTransactionService;
        }

        protected override void ConfigureMapping(IMessagePropertyMapper mapper)
        {
            mapper.ConfigureMapping<StartAllotmentEvent>(x => x.TransactionHash);

            mapper.ConfigureMapping<TransactionCompleted>(m => m.TransactionHash);
            mapper.ConfigureMapping<TransactionFailed>(m => m.TransactionHash);
        }

        protected override string CorrelationPropertyName => nameof(AllotmentEventStartSagaData.TransactionHash);

        public async Task Handle(StartAllotmentEvent command, IMessageHandlerContext context)
        {
            Data.AllotmentEventId = command.AllotmentEventId;

            await _allotmentEventService.SetUpdatingStateAsync(command.AllotmentEventId, true);
            await _ethereumTransactionService.StartAsync(command.TransactionHash, command.UserId, EthereumTransactionType.PublishAllotmentEvent);
            await context.SendLocal(new WaitForTransaction { TransactionHash = command.TransactionHash });
        }

        public async Task Handle(TransactionCompleted message, IMessageHandlerContext context)
        {
            await _allotmentEventService.StartAsync(Data.AllotmentEventId);
            MarkAsComplete();
        }

        public async Task Handle(TransactionFailed message, IMessageHandlerContext context)
        {
            await _allotmentEventService.SetUpdatingStateAsync(Data.AllotmentEventId, false);
            MarkAsComplete();
        }
    }
}