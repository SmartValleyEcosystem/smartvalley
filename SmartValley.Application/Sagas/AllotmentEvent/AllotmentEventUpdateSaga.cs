using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Services;
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
            Data.Name = message.Name;
            Data.TokenContractAddress = message.TokenContractAddress;
            Data.TokenTicker = message.TokenTicker;
            Data.TokenDecimals = message.TokenDecimals;
            Data.FinishDate = message.FinishDate;

            if (string.IsNullOrWhiteSpace(Data.TransactionHash))
            {
                await UpdateAsync();
                return;
            }

            await _transactionService.StartAsync(
                message.TransactionHash,
                message.UserId,
                EthereumTransactionType.EditAllotmentEvent,
                message.AllotmentEventId);

            await context.SendLocal(new WaitForTransaction {TransactionHash = message.TransactionHash});
        }

        public Task Handle(TransactionCompleted message, IMessageHandlerContext context) => UpdateAsync();

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

        private async Task UpdateAsync()
        {
            await _allotmentEventService.UpdateAsync(
                Data.AllotmentEventId,
                Data.Name,
                Data.TokenContractAddress,
                Data.TokenDecimals,
                Data.TokenTicker,
                Data.FinishDate);

            MarkAsComplete();
        }
    }
}