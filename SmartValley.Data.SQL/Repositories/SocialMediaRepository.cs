using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class ProjectSocialMediaRepository : IProjectSocialMediaRepository
    {
        private readonly IReadOnlyDataContext _readContext;
        private readonly IEditableDataContext _editContext;

        public ProjectSocialMediaRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
        {
            _readContext = readContext;
            _editContext = editContext;
        }

        public Task<int> AddRangeAsync(IReadOnlyCollection<ProjectSocialMedia> socialMedias)
        {
            foreach (var entity in socialMedias)
                _editContext.ProjectSocialMedias.Add(entity);
            return _editContext.SaveAsync();
        }
    }
}