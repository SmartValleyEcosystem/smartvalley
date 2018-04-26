using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
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
            => All().FirstOrDefaultAsync(x => x.ProjectId == projectId);

        public Task<ScoringApplication> GetByIdAsync(long id)
            => All().FirstOrDefaultAsync(x => x.Id == id);

        private IIncludableQueryable<ScoringApplication, EthereumTransaction> All()
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