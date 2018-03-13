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
        private readonly ITeamMemberRepository _teamRepository;

        public ProjectService(
            IApplicationRepository applicationRepository,
            IProjectRepository projectRepository,
            IScoringRepository scoringRepository,
            ITeamMemberRepository teamRepository)
        {
            _applicationRepository = applicationRepository;
            _projectRepository = projectRepository;
            _scoringRepository = scoringRepository;
            _teamRepository = teamRepository;
        }

        public async Task<ProjectDetails> GetDetailsAsync(long projectId)
        {
            var project = await FindAsync(projectId);
            var projectScoring = await _scoringRepository.GetByProjectIdAsync(projectId);
            var application = await _applicationRepository.GetByProjectIdAsync(projectId);
            var teamMembers = await _teamRepository.GetAllByApplicationIdAsync(application.Id);

            return new ProjectDetails(project, projectScoring, application, teamMembers);
        }

        public Task<IReadOnlyCollection<ProjectScoring>> GetAllScoredAsync()
            => _projectRepository.GetAllScoredAsync();

        public Task<IReadOnlyCollection<ProjectScoring>> GetByAuthorAsync(Address authorAddress)
            => _projectRepository.GetByAuthorAsync(authorAddress);

        public Task<IReadOnlyCollection<ProjectScoring>> GetForScoringAsync(AreaType areaType, Address expertAddress)
            => _projectRepository.GetForScoringAsync(expertAddress, areaType);

        public async Task<bool> IsAuthorizedToSeeEstimatesAsync(Address account, long projectId)
        {
            var project = await FindAsync(projectId);
            var projectScoring = await _scoringRepository.GetByProjectIdAsync(projectId);

            return projectScoring.Score != null || project.AuthorAddress == account;
        }

        public Task<IReadOnlyCollection<Project>> GetByExternalIdsAsync(IReadOnlyCollection<Guid> externalIds)
        {
            return _projectRepository.GetByExternalIdsAsync(externalIds);
        }

        public Task<IReadOnlyCollection<Project>> GetProjectsByNameAsync(string projectName)
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