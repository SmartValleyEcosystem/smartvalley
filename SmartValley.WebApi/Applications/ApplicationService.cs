using System;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Application.Exceptions;
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
        private readonly ITeamMemberRepository _teamRepository;

        public ApplicationService(
            IApplicationRepository applicationRepository,
            IProjectRepository projectRepository,
            ITeamMemberRepository teamRepository)
        {
            _applicationRepository = applicationRepository;
            _projectRepository = projectRepository;
            _teamRepository = teamRepository;
        }

        public async Task CreateAsync(ApplicationRequest applicationRequest)
        {
            if (applicationRequest == null)
                throw new ArgumentNullException();

            var projectId = await AddProjectAsync(applicationRequest);
            var applicationId = await AddApplicationAsync(applicationRequest, projectId);
            await AddTeamMembersAsync(applicationRequest, applicationId);
        }

        private Task AddTeamMembersAsync(ApplicationRequest applicationRequest, long applicationId)
        {
            var teamMembers = applicationRequest
                .TeamMembers
                .Select(m => CreateTeamMember(m, applicationId))
                .ToArray();

            return _teamRepository.AddRangeAsync(teamMembers);
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

        private async Task<long> AddProjectAsync(ApplicationRequest request)
        {
            var project = new Project
                          {
                              Name = request.Name,
                              Country = request.Country,
                              ProjectArea = request.ProjectArea,
                              Description = request.Description,
                              AuthorAddress = request.AuthorAddress,
                              ExternalId = Guid.Parse(request.ProjectId)
                          };

            await _projectRepository.AddAsync(project);
            return project.Id;
        }
    }
}