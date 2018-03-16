using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IApplicationTeamMemberRepository
    {
        Task<int> AddRangeAsync(IEnumerable<ApplicationTeamMember> teamMembers);

        Task<IReadOnlyCollection<ApplicationTeamMember>> GetAllByApplicationIdAsync(long applicationId);
    }
}