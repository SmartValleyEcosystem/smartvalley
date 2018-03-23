using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartValley.Domain.Interfaces
{
    public interface IScoringApplicationQuestionsRepository
    {
        Task<IReadOnlyCollection<Entities.ScoringApplicationQuestion>> GetAllAsync();
    }
}