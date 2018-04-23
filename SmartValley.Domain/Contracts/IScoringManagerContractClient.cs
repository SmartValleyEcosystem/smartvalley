using System;
using System.Threading.Tasks;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Contracts
{
    public interface IScoringManagerContractClient
    {
        Task<Address> GetScoringAddressAsync(Guid projectExternalId);
    }
}