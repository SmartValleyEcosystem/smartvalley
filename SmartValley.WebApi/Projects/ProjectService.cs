using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Application.AzureStorage;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Projects.Requests;

namespace SmartValley.WebApi.Projects
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ProjectService : IProjectService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IScoringRepository _scoringRepository;
        private readonly IProjectTeamMemberRepository _teamMemberRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ProjectTeamMembersStorageProvider _projectTeamMembersStorageProvider;
        private readonly IProjectSocialMediaRepository _socialMediaRepository;
        private readonly IProjectTeamMemberSocialMediaRepository _projectTeamMemberSocialMediaRepository;

        public ProjectService(
            IApplicationRepository applicationRepository,
            IProjectRepository projectRepository,
            IScoringRepository scoringRepository,
            IProjectTeamMemberRepository teamMemberRepository,
            ICountryRepository countryRepository,
            ProjectTeamMembersStorageProvider projectTeamMembersStorageProvider,
            IProjectSocialMediaRepository socialMediaRepository,
            IProjectTeamMemberSocialMediaRepository projectTeamMemberSocialMediaRepository)
        {
            _applicationRepository = applicationRepository;
            _projectRepository = projectRepository;
            _scoringRepository = scoringRepository;
            _teamMemberRepository = teamMemberRepository;
            _countryRepository = countryRepository;
            _projectTeamMembersStorageProvider = projectTeamMembersStorageProvider;
            _socialMediaRepository = socialMediaRepository;
            _projectTeamMemberSocialMediaRepository = projectTeamMemberSocialMediaRepository;
        }

        public async Task<ProjectDetails> GetDetailsAsync(long projectId)
        {
            var project = await FindAsync(projectId);
            var projectScoring = await _scoringRepository.GetByProjectIdAsync(projectId);
            var application = await _applicationRepository.GetByProjectIdAsync(projectId);
            var teamMembers = await _teamMemberRepository.GetAllByProjectIdAsync(projectId);
            var country = await _countryRepository.GetByIdAsync(project.CountryId);
            var details = new ProjectDetails(project, projectScoring, application, country) {TeamMembers = teamMembers};
            return details;
        }

        public Task<IReadOnlyCollection<ProjectDetails>> GetScoredAsync(SearchProjectsQuery projectsQuery)
            => _projectRepository.GetScoredAsync(projectsQuery);

        public Task<int> GetScoredTotalCountAsync(SearchProjectsQuery projectsQuery)
            => _projectRepository.GetScoredTotalCountAsync(projectsQuery);

        public Task<IReadOnlyCollection<ProjectDetails>> GetByAuthorIdAsync(long authorId)
            => _projectRepository.GetByAuthorIdAsync(authorId);

        public async Task<bool> IsAuthorizedToEditProjectTeamMemberAsync(long userId, long projectTeamMemberId)
        {
            var projectTeamMember = await _teamMemberRepository.GetByIdAsync(projectTeamMemberId);
            var project = await FindAsync(projectTeamMember.ProjectId);
            return project.AuthorId == userId;
        }

        public Task<IReadOnlyCollection<ProjectDetails>> GetForScoringAsync(AreaType areaType, long expertId)
            => _projectRepository.GetForScoringAsync(areaType, expertId);

        public async Task<bool> IsAuthorizedToSeeEstimatesAsync(long userId, long projectId)
        {
            var project = await FindAsync(projectId);
            var projectScoring = await _scoringRepository.GetByProjectIdAsync(projectId);

            return projectScoring.Score != null || project.AuthorId != userId;
        }

        public Task<IReadOnlyCollection<ProjectDetails>> GetByExternalIdsAsync(IReadOnlyCollection<Guid> externalIds)
            => _projectRepository.GetByExternalIdsAsync(externalIds);

        public Task<IReadOnlyCollection<ProjectDetails>> GetProjectsByNameAsync(string projectName)
            => _projectRepository.GetAllByNameAsync(projectName);

        public async Task CreateAsync(long userId, CreateProjectRequest request)
        {
            if (request == null)
                throw new ArgumentNullException();

            var projectId = await AddProjectAsync(userId, request);
            if (request.SocialMedias != null && request.SocialMedias.Any())
                await AddSocialMediasAsync(request, projectId);

            if (request.TeamMembers != null && request.TeamMembers.Any())
            {
                var teamMembers = await AddTeamMembersAsync(request, projectId);
                await AddTeamMembersSocialMediasAsync(request, teamMembers);
            }
        }

        public async Task UpdateTeamMemberPhotoAsync(long projectTeamMemberId, AzureFile photo)
        {
            var photoName = $"project-{projectTeamMemberId}/photo{photo.Extension}";
            await _projectTeamMembersStorageProvider.UploadAsync(photoName, photo);
            await _teamMemberRepository.UpdatePhotoNameAsync(projectTeamMemberId, photoName);
        }

        private async Task<long> AddProjectAsync(long userId, CreateProjectRequest request)
        {
            var country = await _countryRepository.GetByCodeAsync(request.CountryCode);
            if (country == null)
                throw new AppErrorException(ErrorCode.CountryNotFound);

            var project = new Project
                          {
                              Name = request.Name,
                              CountryId = country.Id,
                              CategoryId = (CategoryType) request.CategoryId,
                              Description = request.Description,
                              AuthorId = userId,
                              ExternalId = Guid.Parse(request.ProjectId),
                              ContactEmail = request.ContactEmail,
                              IcoDate = request.IcoDate,
                              Website = request.Website,
                              WhitePaperLink = request.WhitePaperLink,
                              StageId = (StageType) request.StageId,
                          };

            await _projectRepository.AddAsync(project);
            return project.Id;
        }

        private ProjectTeamMember CreateTeamMember(ProjectTeamMemberRequest memberRequest, long projectId)
        {
            return new ProjectTeamMember
                   {
                       ProjectId = projectId,
                       FullName = memberRequest.FullName,
                       About = memberRequest.About,
                       Role = memberRequest.Role
                   };
        }

        private ProjectTeamMemberSocialMedia CreateTeamMemberSocialMedia(SocialMediaRequest socialMediaRequest, long teamMemberId)
        {
            return new ProjectTeamMemberSocialMedia
                   {
                       Url = socialMediaRequest.Link,
                       SocialMediaId = socialMediaRequest.NetworkId,
                       TeamMemberId = teamMemberId
                   };
        }

        private ProjectSocialMedia CreateProjectSocialMedia(SocialMediaRequest socialMediaRequest, long projectId)
        {
            return new ProjectSocialMedia
                   {
                       Url = socialMediaRequest.Link,
                       ProjectId = projectId,
                       SocialMediaId = socialMediaRequest.NetworkId
                   };
        }

        private async Task<ProjectSocialMedia[]> AddSocialMediasAsync(CreateProjectRequest request, long projectId)
        {
            var socialMedias = request
                               .SocialMedias
                               .Select(s => CreateProjectSocialMedia(s, projectId))
                               .ToArray();
            await _socialMediaRepository.AddRangeAsync(socialMedias);
            return socialMedias;
        }

        private Task AddTeamMembersSocialMediasAsync(CreateProjectRequest request, ProjectTeamMember[] projectTeamMembers)
        {
            var facebookSocialMedias = (from memberRequest in request.TeamMembers
                                        join projectTeamMember in projectTeamMembers
                                            on new {memberRequest.FullName, memberRequest.About, memberRequest.Role}
                                            equals new {projectTeamMember.FullName, projectTeamMember.About, projectTeamMember.Role}
                                        select new ProjectTeamMemberSocialMedia
                                               {
                                                   SocialMediaId = SocialMediaType.Facebook,
                                                   Url = memberRequest.FacebookLink,
                                                   TeamMemberId = projectTeamMember.Id
                                               }).ToArray();

            var linkedinSocialMedias = (from memberRequest in request.TeamMembers
                                        join projectTeamMember in projectTeamMembers
                                            on new {memberRequest.FullName, memberRequest.About, memberRequest.Role}
                                            equals new {projectTeamMember.FullName, projectTeamMember.About, projectTeamMember.Role}
                                        select new ProjectTeamMemberSocialMedia
                                               {
                                                   SocialMediaId = SocialMediaType.LinkedIn,
                                                   Url = memberRequest.LinkedInLink,
                                                   TeamMemberId = projectTeamMember.Id
                                               }).ToArray();

            var allMedias = facebookSocialMedias.Union(linkedinSocialMedias).ToArray();

            return _projectTeamMemberSocialMediaRepository.AddRangeAsync(allMedias);
        }

        private async Task<ProjectTeamMember[]> AddTeamMembersAsync(CreateProjectRequest request, long projectId)
        {
            var teamMembers = request
                              .TeamMembers
                              .Select(m => CreateTeamMember(m, projectId))
                              .ToArray();
            await _teamMemberRepository.AddRangeAsync(teamMembers);
            return teamMembers;
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