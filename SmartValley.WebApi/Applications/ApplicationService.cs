using System;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Application.Contracts;
using SmartValley.Application.Exceptions;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.TeamMembers;

namespace SmartValley.WebApi.Applications
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITeamMemberRepository _teamRepository;
        private readonly IProjectManagerContractClient _projectManagerContractClient;

        public ApplicationService(
            IApplicationRepository applicationRepository,
            IProjectRepository projectRepository,
            ITeamMemberRepository teamRepository,
            IProjectManagerContractClient projectManagerContractClient)
        {
            _applicationRepository = applicationRepository;
            _projectRepository = projectRepository;
            _teamRepository = teamRepository;
            _projectManagerContractClient = projectManagerContractClient;
        }

        public async Task CreateAsync(ApplicationRequest applicationRequest)
        {
            if (applicationRequest == null)
                throw new ArgumentNullException();

            var projectAddress = await _projectManagerContractClient.GetProjectAddressAsync(
                                     applicationRequest.ProjectId,
                                     applicationRequest.TransactionHash);

            await CreateProjectAsync(applicationRequest, projectAddress);
        }

        private async Task CreateProjectAsync(ApplicationRequest applicationRequest, string projectContractAddress)
        {
            var project = await AddProjectAsync(applicationRequest, projectContractAddress);

            var application = await AddApplicationAsync(applicationRequest, project.Id);

            var teamMembers = applicationRequest
                .TeamMembers
                .Select(m => CreateTeamMember(m, application.Id)).ToArray();

            await _teamRepository.AddRangeAsync(teamMembers);
        }

        private static TeamMember CreateTeamMember(TeamMemberRequest memberRequest, long applicationId)
        {
            return new TeamMember
                   {
                       ApplicationId = applicationId,
                       FullName = memberRequest.FullName,
                       Type = ToMemberType(memberRequest.MemberType),
                       FacebookLink = memberRequest.FacebookLink,
                       LinkedInLink = memberRequest.LinkedInLink
                   };
        }

        private static TeamMemberType ToMemberType(string memberType)
        {
            if (Enum.TryParse(memberType, out TeamMemberType result))
                return result;

            throw new AppErrorException(ErrorCode.ValidatationError, $"Unknown team member type: '{memberType}'");
        }

        private async Task<Domain.Entities.Application> AddApplicationAsync(ApplicationRequest model, long projectId)
        {
            var application = new Domain.Entities.Application
                              {
                                  ProjectId = projectId,
                                  SoftCap = model.SoftCap,
                                  HardCap = model.HardCap,
                                  BlockchainType = model.BlockChainType,
                                  FinancialModelLink = model.FinanceModelLink,
                                  InvestmentsAreAttracted = model.AttractedInvestments,
                                  ProjectStatus = model.ProjectStatus,
                                  WhitePaperLink = model.WhitePaperLink,
                                  MvpLink = model.MvpLink
                              };

            await _applicationRepository.AddAsync(application);

            return application;
        }

        private async Task<Project> AddProjectAsync(
            ApplicationRequest request,
            string projectContractAddress)
        {
            var project = new Project
                          {
                              Name = request.Name,
                              Country = request.Country,
                              ProjectArea = request.ProjectArea,
                              Description = request.Description,
                              AuthorAddress = request.AuthorAddress,
                              ProjectAddress = projectContractAddress,
                              ExternalId = Guid.Parse(request.ProjectId)
                          };

            await _projectRepository.AddAsync(project);

            return project;
        }
    }
}