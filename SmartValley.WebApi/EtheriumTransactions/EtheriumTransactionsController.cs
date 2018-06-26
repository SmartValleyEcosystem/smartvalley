using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Services;
using SmartValley.WebApi.EtheriumTransactions.Requests;
using SmartValley.WebApi.EtheriumTransactions.Response;
using SmartValley.WebApi.Extensions;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.EtheriumTransactions
{
    [Route("api/transactions")]
    public class EtheriumTransactionsController : Controller
    {
        private readonly IEthereumTransactionService _ethereumTransactionService;

        public EtheriumTransactionsController(IEthereumTransactionService ethereumTransactionService)
        {
            _ethereumTransactionService = ethereumTransactionService;
        }

        [HttpGet]
        public async Task<PartialCollectionResponse<EthereumTransactionResponse>> GetAsync([FromQuery] EtheriumTransactionsQueryRequest request)
        {
            var query = new EtheriumTransactionsQuery(request.Offset,
                                                      request.Count,
                                                      request.UserIds ?? new long[0],
                                                      request.EntityIds ?? new long[0],
                                                      request.EntityTypes ?? new EthereumTransactionEntityType[0],
                                                      request.TransactionTypes ?? new EthereumTransactionType[0],
                                                      request.Statuses ?? new EthereumTransactionStatus[0]);

            var transactions = await _ethereumTransactionService.GetAsync(query);
            return transactions.ToPartialCollectionResponse(EthereumTransactionResponse.Create);
        }
    }
}