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
                               .Include(x => x.Country)
                               .FirstOrDefaultAsync(x => x.ProjectId == projectId);
        }

        public void Add(ScoringApplication scoringApplication)
        {
            _editContext.ScoringApplications.Add(scoringApplication);
        }

        public async Task SaveChangesAsync()
        {
            await _editContext.SaveAsync();
        }
    }
}