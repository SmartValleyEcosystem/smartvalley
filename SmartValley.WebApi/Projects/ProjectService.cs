using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;

namespace SmartValley.WebApi.Projects
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ProjectService : IProjectService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IScoringRepository _scoringRepository;
        private readonly IApplicationTeamMemberRepository _teamRepository;
        private readonly ICountryRepository _countryRepository;

        public ProjectService(
            IApplicationRepository applicationRepository,
            IProjectRepository projectRepository,
            IScoringRepository scoringRepository,
            IApplicationTeamMemberRepository teamRepository,
            ICountryRepository countryRepository)
        {
            _applicationRepository = applicationRepository;
            _projectRepository = projectRepository;
            _scoringRepository = scoringRepository;
            _teamRepository = teamRepository;
            _countryRepository = countryRepository;
        }

        public async Task<ProjectDetails> GetDetailsAsync(long projectId)
        {
            var project = await FindAsync(projectId);
            var projectScoring = await _scoringRepository.GetByProjectIdAsync(projectId);
            var application = await _applicationRepository.GetByProjectIdAsync(projectId);
            var teamMembers = await _teamRepository.GetAllByApplicationIdAsync(application.Id);
            var country = await _countryRepository.GetByIdAsync(project.CountryId);
            var details = new ProjectDetails(project, projectScoring, application, country) {TeamMembers = teamMembers};
            return details;
        }

        public Task<IReadOnlyCollection<ProjectDetails>> GetScoredAsync(int page, int pageSize)
            => _projectRepository.GetScoredAsync(page, pageSize);

        public Task<IReadOnlyCollection<ProjectDetails>> GetByAuthorAsync(Address authorAddress)
            => _projectRepository.GetByAuthorAsync(authorAddress);

        public Task<IReadOnlyCollection<ProjectDetails>> GetForScoringAsync(AreaType areaType, long expertId) 
            => _projectRepository.GetForScoringAsync(areaType, expertId);

        public async Task<bool> IsAuthorizedToSeeEstimatesAsync(Address account, long projectId)
        {
            var project = await FindAsync(projectId);
            var projectScoring = await _scoringRepository.GetByProjectIdAsync(projectId);

            return projectScoring.Score != null || project.AuthorAddress == account;
        }

        public Task<IReadOnlyCollection<ProjectDetails>> GetByExternalIdsAsync(IReadOnlyCollection<Guid> externalIds)
            => _projectRepository.GetByExternalIdsAsync(externalIds);

        public Task<IReadOnlyCollection<ProjectDetails>> GetProjectsByNameAsync(string projectName)
            => _projectRepository.GetAllByNameAsync(projectName);

        private async Task<Project> FindAsync(long projectId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);

            if (project == null)
                throw new AppErrorException(ErrorCode.ProjectNotFound);

            return project;
        }
    }
}