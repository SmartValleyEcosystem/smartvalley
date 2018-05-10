using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using SmartValley.Domain.Services;
using SmartValley.Ethereum;
using SmartValley.Messages.Commands;
using SmartValley.Messages.Events;

namespace SmartValley.Application.Sagas.TransactionHandling
{
    // ReSharper disable once UnusedMember.Global
    public class TransactionHandlingSaga :
        SqlSaga<TransactionHandlingSagaData>,
        IAmStartedByMessages<WaitForTransaction>,
        IHandleTimeouts<TransactionHandlingSaga.PollingTimeout>
    {
        private const int ReceiptPollingIntervalInSeconds = 10;

        private readonly EthereumClient _ethereumClient;
        private readonly IEthereumTransactionService _ethereumTransactionService;

        protected override string CorrelationPropertyName => nameof(TransactionHandlingSagaData.TransactionHash);

        public TransactionHandlingSaga(EthereumClient ethereumClient, IEthereumTransactionService ethereumTransactionService)
        {
            _ethereumClient = ethereumClient;
            _ethereumTransactionService = ethereumTransactionService;
        }

        public Task Handle(WaitForTransaction message, IMessageHandlerContext context)
        {
            Data.TransactionHash = message.TransactionHash;

            return CheckTransactionStatusAsync(context);
        }

        public Task Timeout(PollingTimeout state, IMessageHandlerContext context)
            => CheckTransactionStatusAsync(context);

        private async Task CheckTransactionStatusAsync(IMessageHandlerContext context)
        {
            switch (await _ethereumClient.GetTransactionStatusAsync(Data.TransactionHash))
            {
                case TransactionStatus.NotMined:
                case TransactionStatus.NotConfirmed:
                    await RequestTimeout<PollingTimeout>(context, TimeSpan.FromSeconds(ReceiptPollingIntervalInSeconds));
                    return;
                case TransactionStatus.Completed:
                    await CompleteAsync(context);
                    return;
                case TransactionStatus.Failed:
                    await FailAsync(context);
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async Task CompleteAsync(IMessageHandlerContext context)
        {
            await _ethereumTransactionService.CompleteAsync(Data.TransactionHash);

            var message = new TransactionCompleted {TransactionHash = Data.TransactionHash};
            await context.Publish(message);

            MarkAsComplete();
        }

        private async Task FailAsync(IMessageHandlerContext context)
        {
            await _ethereumTransactionService.FailAsync(Data.TransactionHash);

            var message = new TransactionFailed {TransactionHash = Data.TransactionHash};
            await context.Publish(message);

            MarkAsComplete();
        }

        protected override void ConfigureMapping(IMessagePropertyMapper mapper)
        {
            mapper.ConfigureMapping<WaitForTransaction>(m => m.TransactionHash);
        }

        public class PollingTimeout
        {
        }
    }
}