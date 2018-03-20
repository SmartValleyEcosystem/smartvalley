using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class ProjectTeamMemberRepository : EntityCrudRepository<ProjectTeamMember>, IProjectTeamMemberRepository
    {
        public ProjectTeamMemberRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
            : base(readContext, editContext)
        {
        }

        public async Task<IReadOnlyCollection<ProjectTeamMember>> GetAllByProjectIdAsync(long projectId) =>
            await ReadContext.ProjectTeamMembers
                             .Where(t => t.ProjectId == projectId)
                             .ToArrayAsync();

        public Task UpdatePhotoNameAsync(long id, string photoUrl)
        {
            var projectTeamMember = new ProjectTeamMember
                                    {
                                        Id = id,
                                        PhotoUrl = photoUrl
                                    };

            EditContext.ProjectTeamMembers.Attach(projectTeamMember);
            EditContext.Entity(projectTeamMember).Property(p => p.PhotoUrl).IsModified = true;
            return EditContext.SaveAsync();
        }
    }
}