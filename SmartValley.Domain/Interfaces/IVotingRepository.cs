using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IVotingRepository
    {
        Task<int> AddAsync(Voting voting);
        
        Task<Voting> GetByIdAsync(long votingId);
    }
}