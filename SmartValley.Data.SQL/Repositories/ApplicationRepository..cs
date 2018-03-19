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
            return ReadContext.Applications.FirstAsync(a => a.ProjectId == projectId);
        }

        public async Task<IReadOnlyCollection<Category>> GetCategoriesAsync()
            => await ReadContext.Categories.ToArrayAsync();

        public async Task<IReadOnlyCollection<Stage>> GetStagesAsync()
            => await ReadContext.Stages.ToArrayAsync(); 

        public async Task<IReadOnlyCollection<SocialMedia>> GetSocialMediasAsync()
            => await ReadContext.SocialMedias.ToArrayAsync();
    }
}