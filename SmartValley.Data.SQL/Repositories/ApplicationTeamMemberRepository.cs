using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class ApplicationTeamMemberRepository : EntityCrudRepository<ApplicationTeamMember>, IApplicationTeamMemberRepository
    {
        public ApplicationTeamMemberRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
            : base(readContext, editContext)
        {
        }

        public async Task<IReadOnlyCollection<ApplicationTeamMember>> GetAllByApplicationIdAsync(long applicationId) =>
            await ReadContext.ApplicationTeamMembers
                             .Where(t => t.ApplicationId == applicationId)
                             .ToArrayAsync();
    }
}