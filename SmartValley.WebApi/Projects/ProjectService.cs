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
using SmartValley.WebApi.WebApi;

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
        private readonly ProjectStorageProvider _projectStorageProvider;

        public ProjectService(
            IApplicationRepository applicationRepository,
            IProjectRepository projectRepository,
            IScoringRepository scoringRepository,
            IProjectTeamMemberRepository teamMemberRepository,
            ICountryRepository countryRepository,
            ProjectTeamMembersStorageProvider projectTeamMembersStorageProvider,
            ProjectStorageProvider projectStorageProvider)
        {
            _applicationRepository = applicationRepository;
            _projectRepository = projectRepository;
            _scoringRepository = scoringRepository;
            _teamMemberRepository = teamMemberRepository;
            _countryRepository = countryRepository;
            _projectTeamMembersStorageProvider = projectTeamMembersStorageProvider;
            _projectStorageProvider = projectStorageProvider;
        }

        public async Task<ProjectDetails> GetDetailsAsync(long projectId)
        {
            var project = await FindAsync(projectId);
            var projectScoring = await _scoringRepository.GetByProjectIdAsync(projectId);
            var application = await _applicationRepository.GetByProjectIdAsync(projectId);
            var teamMembers = await _teamMemberRepository.GetByProjectIdAsync(projectId);
            var country = await _countryRepository.GetByIdAsync(project.CountryId);
            var details = new ProjectDetails(project, projectScoring, application, country) {TeamMembers = teamMembers};
            return details;
        }

        public Task<ProjectDetails> GetDetailsByUserIdAsync(long userId)
            => _projectRepository.GetByAuthorIdAsync(userId);

        public Task<IReadOnlyCollection<ProjectTeamMember>> GetTeamAsync(long projectId)
            => _teamMemberRepository.GetByProjectIdAsync(projectId);

        public Task<IReadOnlyCollection<ProjectDetails>> QueryAsync(ProjectsQuery projectsQuery)
            => _projectRepository.QueryAsync(projectsQuery);

        public Task<int> GetQueryTotalCountAsync(ProjectsQuery projectsQuery)
            => _projectRepository.GetQueryTotalCountAsync(projectsQuery);

        public Task<ProjectDetails> GetByAuthorIdAsync(long authorId)
            => _projectRepository.GetByAuthorIdAsync(authorId);

        public async Task<bool> IsAuthorizedToEditProjectTeamMemberAsync(long userId, long projectTeamMemberId)
        {
            var projectTeamMember = await _teamMemberRepository.GetByIdAsync(projectTeamMemberId);
            var project = await FindAsync(projectTeamMember.ProjectId);
            return project.AuthorId == userId;
        }

        public async Task<bool> IsAuthorizedToEditProjectAsync(long projectId, long userId)
        {
            var project = await FindAsync(projectId);
            return project.AuthorId == userId;
        }

        public Task<IReadOnlyCollection<ProjectDetails>> GetForScoringAsync(AreaType areaType, long expertId)
            => _projectRepository.GetForScoringAsync(areaType, expertId);

        public async Task<bool> IsAuthorizedToSeeEstimatesAsync(long userId, long projectId)
        {
            var project = await FindAsync(projectId);
            var scoring = await _scoringRepository.GetByProjectIdAsync(projectId);

            return scoring.Score != null || project.AuthorId != userId;
        }

        public Task<IReadOnlyCollection<ProjectDetails>> GetByExternalIdsAsync(IReadOnlyCollection<Guid> externalIds)
            => _projectRepository.GetByExternalIdsAsync(externalIds);

        public Task<IReadOnlyCollection<ProjectDetails>> GetProjectsByNameAsync(string projectName)
            => _projectRepository.GetAllByNameAsync(projectName);

        public async Task CreateAsync(long userId, CreateProjectRequest request)
        {
            var projectId = await AddProjectAsync(userId, request);

            if (request.TeamMembers != null && request.TeamMembers.Any())
                await AddTeamMembersAsync(request.TeamMembers, projectId);
        }

        public async Task UpdateAsync(long projectId, UpdateProjectRequest request)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            var country = await GetCountryAsync(request.CountryCode);

            project.Name = request.Name;
            project.CountryId = country.Id;
            project.Category = (Category) request.CategoryId;
            project.Description = request.Description;
            project.ContactEmail = request.ContactEmail;
            project.IcoDate = request.IcoDate;
            project.Website = request.Website;
            project.WhitePaperLink = request.WhitePaperLink;
            project.Stage = (Stage) request.StageId;
            project.Facebook = request.Facebook;
            project.Reddit = request.Reddit;
            project.BitcoinTalk = request.BitcoinTalk;
            project.Telegram = request.Telegram;
            project.Github = request.Github;
            project.Medium = request.Medium;
            project.Twitter = request.Twitter;
            project.Linkedin = request.Linkedin;

            await UpdateTeamMembersAsync(request.TeamMembers.Where(t => t.Id != 0).ToArray(), project.Id);
            await AddTeamMembersAsync(request.TeamMembers.Where(t => t.Id == 0).ToArray(), project.Id);
            await _projectRepository.UpdateWholeAsync(project);
        }

        public async Task DeleteAsync(long projectId)
        {
            var scoring = await _scoringRepository.GetByProjectIdAsync(projectId);
            if (scoring != null)
                throw new AppErrorException(ErrorCode.ProjectCouldntBeRemoved);

            var project = await _projectRepository.GetByIdAsync(projectId);

            await _projectRepository.RemoveAsync(project);
        }

        private async Task UpdateTeamMembersAsync(IReadOnlyCollection<ProjectTeamMemberRequest> teamMemberRequests, long projectId)
        {
            var existingTeamMembers = await _teamMemberRepository.GetByProjectIdAsync(projectId);

            foreach (var existingTeamMember in existingTeamMembers)
            {
                var requestTeamMember = teamMemberRequests.FirstOrDefault(t => t.Id == existingTeamMember.Id);
                if (requestTeamMember == null)
                {
                    await _teamMemberRepository.RemoveAsync(existingTeamMember);
                    return;
                }

                existingTeamMember.About = requestTeamMember.About;
                existingTeamMember.FullName = requestTeamMember.FullName;
                existingTeamMember.Role = requestTeamMember.Role;
                existingTeamMember.Facebook = requestTeamMember.Facebook;
                existingTeamMember.Linkedin = requestTeamMember.Linkedin;

                await _teamMemberRepository.UpdateWholeAsync(existingTeamMember);
            }
        }

        public async Task UpdateTeamMemberPhotoAsync(long projectTeamMemberId, AzureFile photo)
        {
            var photoName = $"project-{projectTeamMemberId}/photo-{Guid.NewGuid()}{photo.Extension}";
            var link = await _projectTeamMembersStorageProvider.UploadAndGetUriAsync(photoName, photo);
            await _teamMemberRepository.UpdatePhotoNameAsync(projectTeamMemberId, link);
        }

        public async Task UpdateImageAsync(long projectId, AzureFile image)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            project.ImageUrl = await UploadImageAndGetUrlAsync(projectId, image);
            await _projectRepository.UpdateAsync(project, p => p.ImageUrl);
        }

        public async Task<Project> GetAsync(long projectId)
            => await _projectRepository.GetByIdAsync(projectId) ?? throw new AppErrorException(ErrorCode.ProjectNotFound);

        private async Task<long> AddProjectAsync(long userId, CreateProjectRequest request)
        {
            var country = await GetCountryAsync(request.CountryCode);

            var project = new Project
                          {
                              Name = request.Name,
                              CountryId = country.Id,
                              Category = (Category) request.Category,
                              Description = request.Description,
                              AuthorId = userId,
                              ExternalId = Guid.NewGuid(),
                              ContactEmail = request.ContactEmail,
                              IcoDate = request.IcoDate,
                              Website = request.Website,
                              WhitePaperLink = request.WhitePaperLink,
                              Stage = (Stage) request.Stage,
                              Facebook = request.Facebook,
                              Reddit = request.Reddit,
                              BitcoinTalk = request.BitcoinTalk,
                              Telegram = request.Telegram,
                              Github = request.Github,
                              Medium = request.Medium,
                              Twitter = request.Twitter,
                              Linkedin = request.Linkedin
                          };

            await _projectRepository.AddAsync(project);
            return project.Id;
        }

        private async Task<Country> GetCountryAsync(string code)
        {
            var country = await _countryRepository.GetByCodeAsync(code);
            if (country == null)
                throw new AppErrorException(ErrorCode.CountryNotFound);
            return country;
        }

        private async Task<string> UploadImageAndGetUrlAsync(long projectId, AzureFile image)
        {
            if (image == null)
                return null;

            var imageName = $"project-{projectId}/image-{Guid.NewGuid()}{image.Extension}";
            var imageUrl = await _projectStorageProvider.UploadAndGetUriAsync(imageName, image);
            return imageUrl;
        }

        private async Task<ProjectTeamMember[]> AddTeamMembersAsync(IReadOnlyCollection<ProjectTeamMemberRequest> teamMemberRequests, long projectId)
        {
            var teamMembers = teamMemberRequests.Select(m => new ProjectTeamMember
                                                             {
                                                                 ProjectId = projectId,
                                                                 FullName = m.FullName,
                                                                 About = m.About,
                                                                 Role = m.Role,
                                                                 Facebook = m.Facebook,
                                                                 Linkedin = m.Linkedin
                                                             })
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