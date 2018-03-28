using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class ApplicationRepository : EntityCrudRepository<Application>, IApplicationRepository
    {
        public ApplicationRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
            : base(readContext, editContext)
        {
        }

        public Task<Application> GetByProjectIdAsync(long projectId)
        {
            return ReadContext.Applications.FirstOrDefaultAsync(a => a.ProjectId == projectId);
        }
    }
}