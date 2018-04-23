using System.Collections.Generic;
using System.Linq;
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
        private readonly IReadOnlyDataContext _readContext;

        public ScoringApplicationRepository(IEditableDataContext editContext, IReadOnlyDataContext readContext)
        {
            _editContext = editContext;
            _readContext = readContext;
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

        public Task<ScoringApplication> GetByIdAsync(long id)
        {
            return _readContext.ScoringApplications
                               .Include(x => x.Answers)
                               .Include(x => x.Advisers)
                               .Include(x => x.TeamMembers)
                               .Include(x => x.Country)
                               .FirstOrDefaultAsync(x => x.Id == id);
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