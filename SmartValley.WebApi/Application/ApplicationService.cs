using System;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Application.Exceptions;
using SmartValley.Data.SQL.Repositories;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Application
{
    public class ApplicationService : IApplicationService
    {
        private readonly ApplicationRepository _applicationRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly TeamMemberRepository _teamRepository;

        public ApplicationService(
            ApplicationRepository applicationRepository,
            ProjectRepository projectRepository,
            TeamMemberRepository teamRepository)
        {
            _applicationRepository = applicationRepository;
            _projectRepository = projectRepository;
            _teamRepository = teamRepository;
        }

        public async Task CreateApplicationAsync(ApplicationRequest applicationRequest)
        {
            if (applicationRequest == null)
                throw new ArgumentNullException();

            var project = await AddProjectAsync(applicationRequest);

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

        private async Task<Project> AddProjectAsync(ApplicationRequest model)
        {
            var project = new Project
                          {
                              Name = model.Name,
                              Country = model.Country,
                              ProjectArea = model.ProjectArea,
                              ProblemDesc = model.ProbablyDescription,
                              SolutionDesc = model.SolutionDescription,
                              AuthorAddress = model.AuthorAddress,
                              ProjectAddress = "projAddress"
                          };

            await _projectRepository.AddAsync(project);

            return project;
        }
    }
}