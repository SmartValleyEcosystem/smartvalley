using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class ProjectTeamMemberSocialMediaRepository : IProjectTeamMemberSocialMediaRepository
    {
        private readonly IReadOnlyDataContext _readContext;
        private readonly IEditableDataContext _editContext;

        public ProjectTeamMemberSocialMediaRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
        {
            _readContext = readContext;
            _editContext = editContext;
        }

        public Task<int> AddRangeAsync(IReadOnlyCollection<ProjectTeamMemberSocialMedia> projectTeamMemberSocialMedia)
        {
            foreach (var entity in projectTeamMemberSocialMedia)
                _editContext.ProjectTeamMemberSocialMedias.Add(entity);
            return _editContext.SaveAsync();
        }
    }
}