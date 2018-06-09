using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ScoringApplicationRepository : IScoringApplicationRepository
    {
        private readonly IEditableDataContext _editContext;

        public ScoringApplicationRepository(IEditableDataContext editContext)
        {
            _editContext = editContext;
        }

        public Task<ScoringApplication> GetByProjectIdAsync(long projectId)
            => Entities().FirstOrDefaultAsync(x => x.ProjectId == projectId);

        public async Task<IReadOnlyCollection<ScoringApplication>> GetByProjectIdsAsync(IReadOnlyCollection<long> projectIds)
            => await Entities().Where(x => projectIds.Contains(x.ProjectId)).ToArrayAsync();

        public Task<ScoringApplication> GetByIdAsync(long id)
            => Entities().FirstOrDefaultAsync(x => x.Id == id);

        private IQueryable<ScoringApplication> Entities()
        {
            return _editContext.ScoringApplications
                               .Include(x => x.Answers)
                               .Include(x => x.Advisers)
                               .Include(x => x.TeamMembers)
                               .Include(x => x.Country)
                               .Include(x => x.ScoringStartTransaction);
        }

        public void Add(ScoringApplication scoringApplication)
            => _editContext.ScoringApplications.Add(scoringApplication);

        public Task SaveChangesAsync() => _editContext.SaveAsync();
    }
}