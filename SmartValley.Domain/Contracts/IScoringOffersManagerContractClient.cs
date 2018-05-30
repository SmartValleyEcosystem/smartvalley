using System;
using System.Threading.Tasks;

namespace SmartValley.Domain.Contracts
{
    public interface IScoringOffersManagerContractClient
    {
        Task<ScoringInfo> GetScoringInfoAsync(Guid projectExternalId);
    }
}