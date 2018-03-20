using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages.Internal;
using SmartValley.Application.AzureStorage;

namespace SmartValley.WebApi.WebApi
{
    public class InitializationService
    {
        private readonly ApplicationTeamMembersStorageProvider _applicationTeamMembersStorageProvider;
        private readonly ProjectTeamMembersStorageProvider _projectTeamMembersStorageProvider;
        private readonly ExpertApplicationsStorageProvider _expertApplicationsStorageProvider;
        private readonly ProjectStorageProvider _projectStorageProvider;

        public InitializationService(ApplicationTeamMembersStorageProvider applicationTeamMembersStorageProvider,
                                     ProjectTeamMembersStorageProvider projectTeamMembersStorageProvider,
                                     ExpertApplicationsStorageProvider expertApplicationsStorageProvider,
                                     ProjectStorageProvider projectStorageProvider)
        {
            _applicationTeamMembersStorageProvider = applicationTeamMembersStorageProvider;
            _projectTeamMembersStorageProvider = projectTeamMembersStorageProvider;
            _expertApplicationsStorageProvider = expertApplicationsStorageProvider;
            _projectStorageProvider = projectStorageProvider;
        }

        public Task InitializeAsync()
        {
            return Task.WhenAll(_applicationTeamMembersStorageProvider.InitializeAsync(),
                                _projectTeamMembersStorageProvider.InitializeAsync(),
                                _expertApplicationsStorageProvider.InitializeAsync(),
                                _projectStorageProvider.InitializeAsync());
        }
    }
}