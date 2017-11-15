using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Data.SQL.Repositories;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Application
{
    public class ApplicationService : IApplicationService
    {
        private readonly ApplicationRepository _appRepository;
        private readonly ProjectRepository _projRepository;
        private readonly TeamMemberRepository _teamRepository;

        public ApplicationService(ApplicationRepository appRepository, ProjectRepository projRepository, TeamMemberRepository teamRepository)
        {
            _appRepository = appRepository;
            _projRepository = projRepository;
            _teamRepository = teamRepository;
        }
        public async Task CreateApplication(ApplicationRequest model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }
            if (model.CEO == null)
            {
                throw new ArgumentNullException();
            }
            if (model.CFO == null)
            {
                throw new ArgumentNullException();
            }
            if (model.CMO == null)
            {
                throw new ArgumentNullException();
            }
            if (model.CTO == null)
            {
                throw new ArgumentNullException();
            }
            if (model.PR == null)
            {
                throw new ArgumentNullException();
            }

            var project = new Project
                          {
                Name = model.Name,
                Country = model.Country,
                ProjectArea = model.ProjectArea,
                 ProblemDesc = model.ProbDesc,
                 SolutionDesc = model.SolDesc,
                  AuthorAddress = model.AuthorAddress,
                  ProjectAddress = "projAddress"
            };

            var projectResult = await _projRepository.AddAsync(project);

            var application = new Domain.Entities.Application
                              {
                                  ProjectId = project.Id,
                 SoftCap = decimal.Parse(model.SoftCap),
                 HardCap = decimal.Parse(model.HardCap),
                 CryptoCurrency = model.BlockChainType,
                 FinancialModelLink = model.FinModelLink,
                 InvestmentsAreAttracted = model.AttractInv,
                 ProjectStatus = model.ProjStat,
                 WhitePaperLink = model.WPLink,
                 MVPLink = model.MvpLink
                              };

            var applicationResult = _appRepository.AddAsync(application);

            var team = new List<TeamMember>
                       {
                           new TeamMember {ApplicationId = application.Id, FacebookLink = model.CEO.FbLink, FullName = model.CEO.FullName, LinkedInLink = model.CEO.LinkedInLink, PersonType = MemberType.CEO},
                           new TeamMember {ApplicationId = application.Id, FacebookLink = model.CFO.FbLink, FullName = model.CFO.FullName, LinkedInLink = model.CFO.LinkedInLink, PersonType = MemberType.CFO},
                           new TeamMember {ApplicationId = application.Id, FacebookLink = model.CMO.FbLink, FullName = model.CMO.FullName, LinkedInLink = model.CMO.LinkedInLink, PersonType = MemberType.CMO},
                           new TeamMember {ApplicationId = application.Id, FacebookLink = model.CTO.FbLink, FullName = model.CTO.FullName, LinkedInLink = model.CTO.LinkedInLink, PersonType = MemberType.CTO},
                           new TeamMember {ApplicationId = application.Id, FacebookLink = model.PR.FbLink, FullName = model.PR.FullName, LinkedInLink = model.PR.LinkedInLink, PersonType = MemberType.PR}
                       };

            var teamResult = await _teamRepository.AddRangeAsync(team);
        }
    }
}
