using System.Linq;
using System.Threading.Tasks;
using SmartValley.Application.Exceptions;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Applications;
using SmartValley.WebApi.TeamMembers;

namespace SmartValley.WebApi.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ITeamMemberRepository _teamRepository;

        public ProjectService(IApplicationRepository applicationRepository, IProjectRepository projectRepository, ITeamMemberRepository teamRepository)
        {
            _applicationRepository = applicationRepository;
            _projectRepository = projectRepository;
            _teamRepository = teamRepository;
        }

        public async Task<ProjectDetailsResponse> GetProjectDetailsByIdAsync(long projectId)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
            {
                throw new AppErrorException(ErrorCode.ProjectDoesntExist);
            }
            var application = await _applicationRepository.GetByProjectIdAsync(projectId);
            var teamMembers = await _teamRepository.GetAllByApplicationId(application.Id);
            var applicationResponse = new ProjectDetailsResponse
                                      {
                                          Name = project.Name,
                                          Description = project.Description,
                                          AuthorAddress = project.AuthorAddress,
                                          Country = project.Country,
                                          Area = project.ProjectArea,
                                          Score = project.Score,

                                          AttractedInvestments = application.InvestmentsAreAttracted,
                                          BlockChainType = application.CryptoCurrency,
                                          FinanceModelLink = application.FinancialModelLink,
                                          HardCap = application.HardCap,
                                          SoftCap = application.SoftCap,
                                          MvpLink = application.MVPLink,
                                          Status = application.ProjectStatus,
                                          WhitePaperLink = application.WhitePaperLink,
                                          TeamMembers = teamMembers.Select(t => new TeamMemberResponse
                                                                                {
                                                                                    FacebookLink = t.FacebookLink,
                                                                                    LinkedInLink = t.LinkedInLink,
                                                                                    FullName = t.FullName,
                                                                                    MemberType = t.Type.FromDomain()
                                                                                }).ToList()
                                      };

            return applicationResponse;
        }
    }
}