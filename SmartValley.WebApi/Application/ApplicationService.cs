using System;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Application.Contracts;
using SmartValley.Application.Exceptions;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.WebApi.Application
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

        public async Task CreateApplicationAsync(ApplicationRequest applicationRequest)
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
                       PersonType = ToPersonType(memberRequest.MemberType),
                       FacebookLink = memberRequest.FacebookLink,
                       LinkedInLink = memberRequest.LinkedInLink
                   };
        }

        private static MemberType ToPersonType(string memberType)
        {
            if (Enum.TryParse(memberType, out MemberType result))
                return result;

            throw new AppErrorException(ErrorCode.ValidatationError, $"Unknown team member type: '{memberType}'");
        }

        private async Task<Domain.Entities.Application> AddApplicationAsync(ApplicationRequest model, long projectId)
        {
            var application = new Domain.Entities.Application
                              {
                                  ProjectId = projectId,
                                  SoftCap = decimal.Parse(model.SoftCap),
                                  HardCap = decimal.Parse(model.HardCap),
                                  CryptoCurrency = model.BlockChainType,
                                  FinancialModelLink = model.FinanceModelLink,
                                  InvestmentsAreAttracted = model.AttractedInvestnemts,
                                  ProjectStatus = model.ProjectStatus,
                                  WhitePaperLink = model.WhitePaperLink,
                                  MVPLink = model.MvpLink
                              };

            await _applicationRepository.AddAsync(application);

            return application;
        }

        private async Task<Domain.Entities.Project> AddProjectAsync(
            ApplicationRequest request,
            string projectContractAddress)
        {
            var project = new Domain.Entities.Project
                          {
                              Name = request.Name,
                              Country = request.Country,
                              ProjectArea = request.ProjectArea,
                              ProblemDesc = request.ProbablyDescription,
                              SolutionDesc = request.SolutionDescription,
                              AuthorAddress = request.AuthorAddress,
                              ProjectAddress = projectContractAddress,
                              ExternalId = Guid.Parse(request.ProjectId)
                          };

            await _projectRepository.AddAsync(project);

            return project;
        }
    }
}