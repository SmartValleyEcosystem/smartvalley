using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class ProjectRepository : EntityCrudRepository<Project>, IProjectRepository
    {
        public ProjectRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
            : base(readContext, editContext)
        {
        }

        public async Task<IReadOnlyCollection<Project>> GetAllScoredAsync() => await ReadContext.Projects.Where(project => project.Score.HasValue).ToArrayAsync();

        public async Task<IEnumerable<Project>> GetAllByAuthorAddressAsync(string address)
        {
            return await ReadContext.Projects
                                    .Where(project => project.AuthorAddress.Equals(address, StringComparison.OrdinalIgnoreCase))
                                    .ToArrayAsync();
        }
    }
}