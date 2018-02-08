using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain;
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

        public Task<IReadOnlyCollection<ProjectScoring>> GetByAuthorAsync(string authorAddress)
            => _projectRepository.GetByAuthorAsync(authorAddress);

        public Task<IReadOnlyCollection<ProjectScoring>> GetForScoringAsync(ExpertiseAreaType expertiseAreaType, string expertAddress)
            => _projectRepository.GetForScoringAsync(expertAddress, expertiseAreaType);

        public async Task<bool> IsAuthorizedToSeeEstimatesAsync(string account, long projectId)
        {
            var project = await FindAsync(projectId);
            var projectScoring = await _scoringRepository.GetByProjectIdAsync(projectId);

            return projectScoring.Score != null || project.AuthorAddress.Equals(account, StringComparison.InvariantCultureIgnoreCase);
        }

        public Task<IReadOnlyCollection<Project>> GetByExternalIdsAsync(IReadOnlyCollection<Guid> externalIds)
        {
            return _projectRepository.GetByExternalIdsAsync(externalIds);
        }

        private async Task<Project> FindAsync(long projectId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);

            if (project == null)
                throw new AppErrorException(ErrorCode.ProjectNotFound);

            return project;
        }
    }
}