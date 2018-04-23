using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using SmartValley.Messages.Commands;

namespace SmartValley.Application.Sagas
{
    // ReSharper disable once UnusedMember.Global
    public class TransactionHandlingSaga :
        SqlSaga<TransactionHandlingSagaData>,
        IAmStartedByMessages<WaitForTransaction>
    {
        protected override string CorrelationPropertyName => nameof(TransactionHandlingSagaData.CorrelationId);

        public Task Handle(WaitForTransaction message, IMessageHandlerContext context)
        {
            MarkAsComplete();
            return Task.CompletedTask;
        }

        protected override void ConfigureMapping(IMessagePropertyMapper mapper)
        {
            mapper.ConfigureMapping<WaitForTransaction>(m => m.CorrelationId);
        }
    }
}