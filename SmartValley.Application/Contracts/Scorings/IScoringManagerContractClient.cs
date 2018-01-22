using System.Threading.Tasks;

namespace SmartValley.Application.Contracts.Scorings
{
    public interface IScoringManagerContractClient
    {
        Task<string> GetScoringAddressAsync(string projectIdString);
    }
}