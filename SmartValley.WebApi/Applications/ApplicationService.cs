using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Applications.Requests;

namespace SmartValley.WebApi.Applications
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IApplicationTeamMemberRepository _teamRepository;
        private readonly ICountryRepository _countryRepository;

        public ApplicationService(
            IApplicationRepository applicationRepository,
            IProjectRepository projectRepository,
            IApplicationTeamMemberRepository teamRepository,
            ICountryRepository countryRepository)
        {
            _applicationRepository = applicationRepository;
            _projectRepository = projectRepository;
            _teamRepository = teamRepository;
            _countryRepository = countryRepository;
        }

        public async Task CreateAsync(long userId, ApplicationRequest applicationRequest)
        {
            if (applicationRequest == null)
                throw new ArgumentNullException();

            var projectId = await AddProjectAsync(userId, applicationRequest);
            var applicationId = await AddApplicationAsync(applicationRequest, projectId);
            await AddTeamMembersAsync(applicationRequest, applicationId);
        }

        public Task<IReadOnlyCollection<Country>> GetCountriesAsync()
            => _countryRepository.GetAllAsync();

        public Task<IReadOnlyCollection<Category>> GetCategoriesAsync()
            => _applicationRepository.GetCategoriesAsync();

        public Task<IReadOnlyCollection<Stage>> GetStagesAsync()
            => _applicationRepository.GetStagesAsync();

        public Task<IReadOnlyCollection<SocialMedia>> GetSocialMediasAsync()
            => _applicationRepository.GetSocialMediasAsync();

        private Task AddTeamMembersAsync(ApplicationRequest applicationRequest, long applicationId)
        {
            var teamMembers = applicationRequest
                .TeamMembers
                .Select(m => CreateTeamMember(m, applicationId))
                .ToArray();

            return _teamRepository.AddRangeAsync(teamMembers);
        }

        private static ApplicationTeamMember CreateTeamMember(ApplicationTeamMemberRequest memberRequest, long applicationId)
        {
            return new ApplicationTeamMember
                   {
                       ApplicationId = applicationId,
                       FullName = memberRequest.FullName,
                       About = memberRequest.About,
                       Role = memberRequest.Role
                   };
        }

        private async Task<long> AddApplicationAsync(ApplicationRequest request, long projectId)
        {
            var application = new Domain.Entities.Application
                              {
                                  ProjectId = projectId,
                                  SoftCap = request.SoftCap,
                                  HardCap = request.HardCap,
                                  BlockchainType = request.BlockChainType,
                                  FinancialModelLink = request.FinanceModelLink,
                                  InvestmentsAreAttracted = request.AttractedInvestments,
                                  ProjectStatus = request.ProjectStatus,
                                  WhitePaperLink = request.WhitePaperLink,
                                  MvpLink = request.MvpLink
                              };

            await _applicationRepository.AddAsync(application);
            return application.Id;
        }

        private async Task<long> AddProjectAsync(long userId, ApplicationRequest request)
        {
            var country = await _countryRepository.GetByCodeAsync(request.CountryCode);

            var project = new Project
                          {
                              Name = request.Name,
                              CountryId = country.Id,
                              CategoryId = (CategoryType) request.CategoryType,
                              Description = request.Description,
                              AuthorId = userId,
                              ExternalId = Guid.Parse(request.ProjectId)
                          };

            await _projectRepository.AddAsync(project);
            return project.Id;
        }
    }
}