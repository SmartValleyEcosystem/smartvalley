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

        public Task<List<Project>> GetAllByCategory(ScoringCategory scoringCategory)
        {
          var projects = from estimate in ReadContext.Estimates
                           join project in ReadContext.Projects on estimate.ProjectId equals project.Id
                           where estimate.ScoringCategory == scoringCategory
                           select project;

            return projects.ToListAsync();
        }
    }
}