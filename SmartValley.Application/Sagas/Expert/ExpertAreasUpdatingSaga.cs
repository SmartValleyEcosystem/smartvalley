using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Services;
using SmartValley.Messages.Commands;
using SmartValley.Messages.Events;

namespace SmartValley.Application.Sagas.Expert
{
    // ReSharper disable once UnusedMember.Global
    public class ExpertAreasUpdatingSaga :
        SqlSaga<ExpertAreasUpdatingSagaData>,
        IAmStartedByMessages<UpdateExpertAreas>,
        IHandleMessages<TransactionCompleted>,
        IHandleMessages<TransactionFailed>
    {
        private readonly IExpertService _expertService;
        private readonly IEthereumTransactionService _transactionService;

        public ExpertAreasUpdatingSaga(IExpertService expertService, IEthereumTransactionService transactionService)
        {
            _expertService = expertService;
            _transactionService = transactionService;
        }

        protected override string CorrelationPropertyName => nameof(ExpertAreasUpdatingSagaData.TransactionHash);

        public async Task Handle(UpdateExpertAreas message, IMessageHandlerContext context)
        {
            Data.TransactionHash = message.TransactionHash;

            var expert = await _expertService.GetByAddressAsync(message.ExpertAddress);
            Data.ExpertId = expert.UserId;

            await _transactionService.StartAsync(message.TransactionHash, message.UserId, EthereumTransactionEntityType.Expert, expert.UserId, EthereumTransactionType.UpdateExpertAreas);

            await context.SendLocal(new WaitForTransaction {TransactionHash = message.TransactionHash});
        }

        public async Task Handle(TransactionCompleted message, IMessageHandlerContext context)
        {
            await _expertService.UpdateExpertAreasAsync(Data.ExpertId);
            MarkAsComplete();
        }

        public Task Handle(TransactionFailed message, IMessageHandlerContext context)
        {
            MarkAsComplete();
            return Task.CompletedTask;
        }

        protected override void ConfigureMapping(IMessagePropertyMapper mapper)
        {
            mapper.ConfigureMapping<UpdateExpertAreas>(m => m.TransactionHash);

            mapper.ConfigureMapping<TransactionCompleted>(m => m.TransactionHash);
            mapper.ConfigureMapping<TransactionFailed>(m => m.TransactionHash);
        }
    }
}