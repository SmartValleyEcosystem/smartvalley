using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Domain.Services
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EthereumTransactionService : IEthereumTransactionService
    {
        private readonly IEthereumTransactionRepository _repository;

        public EthereumTransactionService(IEthereumTransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task CompleteAsync(string hash)
        {
            var transaction = await _repository.GetByHashAsync(hash) ?? throw new InvalidOperationException($"Transaction '{hash}' not found");

            transaction.Complete();

            await _repository.SaveChangesAsync();
        }

        public async Task FailAsync(string hash)
        {
            var transaction = await _repository.GetByHashAsync(hash) ?? throw new InvalidOperationException($"Transaction '{hash}' not found");

            transaction.Fail();

            await _repository.SaveChangesAsync();
        }

        public async Task AddAsync(EthereumTransaction ethereumTransaction)
        {
            _repository.Add(ethereumTransaction);
            await _repository.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<EthereumTransaction>> GetByAllotmentEventIdAsync(long allotmentId)
        {
            return await _repository.GetByAllotmentEventIdAsync(allotmentId);
        }
    }
}