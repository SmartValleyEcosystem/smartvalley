using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IProjectTeamMemberSocialMediaRepository
    {
        Task<int> AddRangeAsync(IReadOnlyCollection<ProjectTeamMemberSocialMedia> projectTeamMemberSocialMedia);
    }
}