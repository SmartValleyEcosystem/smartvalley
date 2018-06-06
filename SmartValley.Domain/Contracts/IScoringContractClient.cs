using System.Threading.Tasks;

namespace SmartValley.Domain.Contracts
{
    public interface IScoringContractClient
    {
        Task<ScoringResults> GetResultsAsync(string scoringAddress);
    }
}