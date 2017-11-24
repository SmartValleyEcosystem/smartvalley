using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class TeamMemberRepository : EntityCrudRepository<TeamMember>, ITeamMemberRepository
    {
        public TeamMemberRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
            : base(readContext, editContext)
        {
        }

        public async Task<IReadOnlyCollection<TeamMember>> GetAllByApplicationId(long applicationId) =>
            await ReadContext.TeamMembers
                             .Where(t => t.ApplicationId == applicationId)
                             .ToArrayAsync();
    }
}