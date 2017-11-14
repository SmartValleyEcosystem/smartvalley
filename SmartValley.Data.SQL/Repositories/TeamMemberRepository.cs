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
    }
}
