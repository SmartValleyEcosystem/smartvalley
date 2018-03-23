using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class ScoringApplicationQuestionsRepository : IScoringApplicationQuestionsRepository
    {
        private readonly IReadOnlyDataContext _readContext;

        public ScoringApplicationQuestionsRepository(IReadOnlyDataContext readContext)
        {
            _readContext = readContext;
        }

        public async Task<IReadOnlyCollection<Domain.Entities.ScoringApplicationQuestion>> GetAllAsync() 
            => await _readContext.ScoringApplicationQuestions.ToArrayAsync();
    }
}