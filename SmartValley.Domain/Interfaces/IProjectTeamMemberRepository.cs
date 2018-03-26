using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IProjectTeamMemberRepository
    {
        Task<int> AddRangeAsync(IEnumerable<ProjectTeamMember> teamMembers);

        Task<IReadOnlyCollection<ProjectTeamMember>> GetByProjectIdAsync(long projectId);

        Task UpdatePhotoNameAsync(long id, string photoUrl);

        Task<ProjectTeamMember> GetByIdAsync(long id);

        Task<int> RemoveAsync(ProjectTeamMember teamMember);

        Task<int> UpdateAsync(ProjectTeamMember teamMember);
    }
}