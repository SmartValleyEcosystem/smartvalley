using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class VotingProjectRepository : IVotingProjectRepository
    {
        private readonly IEditableDataContext _editableContext;

        public VotingProjectRepository(IEditableDataContext editableContext)
        {
            _editableContext = editableContext;
        }

        public Task AddRangeAsync(IEnumerable<VotingProject> votingProjects)
        {
            foreach (var votingProject in votingProjects)
                _editableContext.DbSet<VotingProject>().Add(votingProject);

            return _editableContext.SaveAsync();
        }
    }
}