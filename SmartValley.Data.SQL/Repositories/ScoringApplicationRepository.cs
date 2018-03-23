using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class ScoringApplicationRepository : IScoringApplicationRepository
    {
        private readonly IEditableDataContext _editContext;

        public ScoringApplicationRepository(IEditableDataContext editContext)
        {
            _editContext = editContext;
        }
        
        public Task<ScoringApplication> GetByProjectIdAsync(long projectId)
        {
            return _editContext.ScoringApplications
                                     .Include(x => x.Answers)
                                     .Include(x => x.Advisers)
                                     .Include(x => x.TeamMembers)
                                     .FirstOrDefaultAsync(x => x.ProjectId == projectId);
        }

        public async Task SaveAsync(ScoringApplication scoringApplication)
        {
            await _editContext.SaveAsync();
        }
    }
}