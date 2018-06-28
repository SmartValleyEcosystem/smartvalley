using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Services;
using SmartValley.Messages;
using SmartValley.Messages.Commands;
using SmartValley.Messages.Events;

namespace SmartValley.Application.Sagas.AllotmentEvent
{
    // ReSharper disable once UnusedMember.Global
    public class AllotmentEventUpdateSaga :
        SqlSaga<AllotmentEventUpdateSagaData>,
        IAmStartedByMessages<UpdateAllotmentEvent>,
        IHandleMessages<TransactionCompleted>,
        IHandleMessages<TransactionFailed>
    {
        private readonly IEthereumTransactionService _transactionService;
        private readonly IAllotmentEventService _allotmentEventService;

        public AllotmentEventUpdateSaga(
            IEthereumTransactionService transactionService,
            IAllotmentEventService allotmentEventService)
        {
            _transactionService = transactionService;
            _allotmentEventService = allotmentEventService;
        }

        protected override string CorrelationPropertyName => nameof(AllotmentEventUpdateSagaData.TransactionHash);

        public async Task Handle(UpdateAllotmentEvent message, IMessageHandlerContext context)
        {
            Data.AllotmentEventId = message.AllotmentEventId;
            Data.TransactionHash = message.TransactionHash;

            var ethereumTransactionType = GetTransactionType(message.Operation);
            await _transactionService.StartAsync(message.TransactionHash, message.UserId, EthereumTransactionEntityType.AllotmentEvent, message.AllotmentEventId, ethereumTransactionType);

            await context.SendLocal(new WaitForTransaction {TransactionHash = message.TransactionHash});
        }

        public async Task Handle(TransactionCompleted message, IMessageHandlerContext context)
        {
            await _allotmentEventService.UpdateAsync(Data.AllotmentEventId);
            MarkAsComplete();
        }

        public Task Handle(TransactionFailed message, IMessageHandlerContext context)
        {
            MarkAsComplete();
            return Task.CompletedTask;
        }

        protected override void ConfigureMapping(IMessagePropertyMapper mapper)
        {
            mapper.ConfigureMapping<UpdateAllotmentEvent>(m => m.TransactionHash);

            mapper.ConfigureMapping<TransactionCompleted>(m => m.TransactionHash);
            mapper.ConfigureMapping<TransactionFailed>(m => m.TransactionHash);
        }

        private static EthereumTransactionType GetTransactionType(AllotmentEventOperation operation)
        {
            switch (operation)
            {
                case AllotmentEventOperation.Start:
                    return EthereumTransactionType.StartAllotmentEvent;
                case AllotmentEventOperation.PlaceBid:
                    return EthereumTransactionType.PlaceAllotmentEventBid;
                case AllotmentEventOperation.ReceiveShare:
                    return EthereumTransactionType.ReceiveAllotmentEventShare;
                case AllotmentEventOperation.Edit:
                    return EthereumTransactionType.EditAllotmentEvent;
                case AllotmentEventOperation.Delete:
                    return EthereumTransactionType.DeleteAllotmentEvent;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
            }
        }
    }
}