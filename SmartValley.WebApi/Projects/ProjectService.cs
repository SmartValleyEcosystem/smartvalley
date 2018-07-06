using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Application.AzureStorage;
using SmartValley.Domain;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Projects.Requests;

namespace SmartValley.WebApi.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IScoringOffersRepository _scoringOffersRepository;
        private readonly ProjectTeamMembersStorageProvider _projectTeamMembersStorageProvider;
        private readonly ProjectStorageProvider _projectStorageProvider;
        private readonly IClock _clock;
        private readonly IUserRepository _userRepository;

        public ProjectService(
            IProjectRepository projectRepository,
            IUserRepository userRepository,
            ICountryRepository countryRepository,
            IScoringOffersRepository scoringOffersRepository,
            ProjectTeamMembersStorageProvider projectTeamMembersStorageProvider,
            ProjectStorageProvider projectStorageProvider,
            IClock clock)
        {
            _projectRepository = projectRepository;
            _countryRepository = countryRepository;
            _scoringOffersRepository = scoringOffersRepository;
            _projectTeamMembersStorageProvider = projectTeamMembersStorageProvider;
            _projectStorageProvider = projectStorageProvider;
            _clock = clock;
            _userRepository = userRepository;
        }

        public Task<PagingCollection<Project>> GetAsync(ProjectsQuery query)
            => _projectRepository.GetAsync(query);

        public Task<Project> GetByAuthorIdAsync(long authorId)
            => _projectRepository.GetByAuthorIdAsync(authorId);

        public async Task<bool> IsAuthorizedToEditProjectTeamMemberAsync(long userId, long projectId)
        {
            var project = await GetByIdAsync(projectId);
            return project.AuthorId == userId;
        }

        public async Task<bool> IsAuthorizedToEditProjectAsync(long projectId, long userId)
        {
            var project = await GetByIdAsync(projectId);
            return project.AuthorId == userId;
        }

        public async Task<bool> IsAuthorizedToSeeProjectAsync(long projectId, long? userId)
        {
            var project = await GetByIdAsync(projectId);
            if (!project.IsPrivate)
                return true;

            if (!userId.HasValue)
                return false;

            if (project.AuthorId == userId.Value)
                return true;

            var user = await _userRepository.GetByIdAsync(userId.Value);
            var isAdmin = await _userRepository.HasRoleAsync(user.Address, RoleType.Admin);
            if (isAdmin)
                return true;

            if (project.Scoring == null)
            {
                return false;
            }

            var offersQuery = new OffersQuery(0, 1, expertId: userId, scoringId: project.Scoring.Id);
            var offers = await _scoringOffersRepository.GetAsync(offersQuery, _clock.UtcNow);

            return offers.Any();
        }

        public async Task<Project> CreateAsync(long userId, CreateProjectRequest request)
        {
            var project = await AddProjectAsync(userId, request);

            if (request.TeamMembers != null && request.TeamMembers.Any())
                UpdateTeamMembers(request.TeamMembers, project);

            await _projectRepository.SaveChangesAsync();

            return project;
        }

        public async Task<Project> UpdateAsync(long projectId, UpdateProjectRequest request)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            var country = await GetCountryAsync(request.CountryCode);

            project.Name = request.Name;
            project.CountryId = country.Id;
            project.Category = (Category) request.Category;
            project.Description = request.Description;
            project.ContactEmail = request.ContactEmail;
            project.IcoDate = request.IcoDate;
            project.Website = request.Website;
            project.WhitePaperLink = request.WhitePaperLink;
            project.Stage = (Stage) request.Stage;
            project.Facebook = request.Facebook;
            project.Reddit = request.Reddit;
            project.BitcoinTalk = request.BitcoinTalk;
            project.Telegram = request.Telegram;
            project.Github = request.Github;
            project.Medium = request.Medium;
            project.Twitter = request.Twitter;
            project.Linkedin = request.Linkedin;

            UpdateTeamMembers(request.TeamMembers, project);
            await _projectRepository.SaveChangesAsync();

            return project;
        }

        public async Task DeleteAsync(long projectId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project.Scoring != null)
                throw new AppErrorException(ErrorCode.ProjectCouldntBeRemoved);

            _projectRepository.Remove(project);

            await _projectRepository.SaveChangesAsync();
        }

        private void UpdateTeamMembers(IReadOnlyCollection<ProjectTeamMemberRequest> teamMemberRequests, Project project)
        {
            var teamMembers = teamMemberRequests.Select(m => new ProjectTeamMember(m.Id, m.FullName, m.Role, m.About, m.Facebook, m.Linkedin)).ToArray();
            project.UpdateMembers(teamMembers);
        }

        public async Task UpdateTeamMemberPhotoAsync(long projectId, long projectTeamMemberId, AzureFile photo)
        {
            var photoName = $"project-{projectTeamMemberId}/photo-{Guid.NewGuid()}{photo.Extension}";
            var link = await _projectTeamMembersStorageProvider.UploadAndGetUriAsync(photoName, photo);
            var project = await GetByIdAsync(projectId);
            project.UpdateTeamMemberPhotoLink(projectTeamMemberId, link);
            await _projectRepository.SaveChangesAsync();
        }

        public async Task DeleteTeamMemberPhotoAsync(long projectId, long projectTeamMemberId)
        {
            var project = await GetByIdAsync(projectId);
            var teamMember = project.GetTeamMember(projectTeamMemberId);
            if (teamMember == null)
                throw new AppErrorException(ErrorCode.TeamMemberNotFound);
            if (teamMember.PhotoUrl == null)
                return;
            var photoName = teamMember.PhotoUrl.Split("project-team-members/")[1];
            await _projectTeamMembersStorageProvider.DeleteAsync(photoName);
            project.UpdateTeamMemberPhotoLink(projectTeamMemberId, null);
            await _projectRepository.SaveChangesAsync();
        }

        public async Task UpdateImageAsync(long projectId, AzureFile image)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            project.ImageUrl = await UploadImageAndGetUrlAsync(projectId, image);
            await _projectRepository.SaveChangesAsync();
        }

        public async Task DeleteProjectImageAsync(long projectId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
                throw new AppErrorException(ErrorCode.ProjectNotFound);
            if (project.ImageUrl == null)
                return;
            var projectImageName = project.ImageUrl.Split("projects/")[1];
            project.ImageUrl = null;
            await _projectStorageProvider.DeleteAsync(projectImageName);
            await _projectRepository.SaveChangesAsync();
        }

        public async Task<Project> GetByIdAsync(long projectId)
            => await _projectRepository.GetByIdAsync(projectId) ?? throw new AppErrorException(ErrorCode.ProjectNotFound);

        private async Task<Project> AddProjectAsync(long userId, CreateProjectRequest request)
        {
            var country = await GetCountryAsync(request.CountryCode);
            var user = await _userRepository.GetByIdAsync(userId);

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
                              Linkedin = request.Linkedin,
                              IsPrivate = user.CanCreatePrivateProjects
                          };

            _projectRepository.Add(project);
            return project;
        }

        private async Task<Country> GetCountryAsync(string code)
            => await _countryRepository.GetByCodeAsync(code) ?? throw new AppErrorException(ErrorCode.CountryNotFound);

        private Task<string> UploadImageAndGetUrlAsync(long projectId, AzureFile image)
        {
            if (image == null)
                return null;

            var imageName = $"project-{projectId}/image-{Guid.NewGuid()}{image.Extension}";
            return _projectStorageProvider.UploadAndGetUriAsync(imageName, image);
        }
    }
}