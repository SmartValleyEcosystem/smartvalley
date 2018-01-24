using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class VotingProjectRepository : IVotingProjectRepository
    {
        private readonly IEditableDataContext _editContext;
        private readonly IReadOnlyDataContext _readContext;

        public VotingProjectRepository(
            IEditableDataContext editContext,
            IReadOnlyDataContext readContext)
        {
            _editContext = editContext;
            _readContext = readContext;
        }

        public Task AddRangeAsync(IEnumerable<VotingProject> votingProjects)
        {
            foreach (var votingProject in votingProjects)
                _editContext.DbSet<VotingProject>().Add(votingProject);

            return _editContext.SaveAsync();
        }

        public Task<VotingProject> GetByProjectAsync(long projectId)
            => _readContext.VotingProjects.FirstOrDefaultAsync(vp => vp.ProjectId == projectId);
    }
}