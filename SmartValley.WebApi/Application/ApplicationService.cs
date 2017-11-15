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

            var projectResult = await _projRepository.AddAsync(project);

            var application = new Domain.Entities.Application
                              {
                                  ProjectId = project.Id,
                                  SoftCap = decimal.Parse(model.SoftCap),
                                  HardCap = decimal.Parse(model.HardCap),
                                  CryptoCurrency = model.BlockChainType,
                                  FinancialModelLink = model.FinanceModelLink,
                                  InvestmentsAreAttracted = model.AttractedInvestnemts,
                                  ProjectStatus = model.ProjectStatus,
                                  WhitePaperLink = model.WhitePaperLink,
                                  MVPLink = model.MvpLink
                              };

            var applicationResult = _appRepository.AddAsync(application);
        }
    }
}