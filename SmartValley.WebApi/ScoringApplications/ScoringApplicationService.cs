using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.ScoringApplications.Requests;

namespace SmartValley.WebApi.ScoringApplications
{
    public class ScoringApplicationService : IScoringApplicationService
    {
        private readonly IScoringApplicationRepository _scoringApplicationRepository;
        private readonly IScoringApplicationQuestionsRepository _scoringApplicationQuestionsRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IClock _clock;

        public ScoringApplicationService(IScoringApplicationRepository scoringApplicationRepository,
                                         IScoringApplicationQuestionsRepository scoringApplicationQuestionsRepository,
                                         ICountryRepository countryRepository,
                                         IProjectRepository projectRepository,
                                         IClock clock)
        {
            _scoringApplicationRepository = scoringApplicationRepository;
            _scoringApplicationQuestionsRepository = scoringApplicationQuestionsRepository;
            _countryRepository = countryRepository;
            _projectRepository = projectRepository;
            _clock = clock;
        }

        public Task<IReadOnlyCollection<ScoringApplicationQuestion>> GetQuestionsAsync()
            => _scoringApplicationQuestionsRepository.GetAllAsync();

        public Task<Domain.ScoringApplication> GetApplicationAsync(long projectId)
            => _scoringApplicationRepository.GetByProjectIdAsync(projectId);

        public async Task SaveApplicationAsync(long projectId, SaveScoringApplicationRequest saveScoringApplicationRequest)
        {
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
                throw new AppErrorException(ErrorCode.ProjectNotFound);

            var scoringApplication = await _scoringApplicationRepository.GetByProjectIdAsync(projectId);

            if (scoringApplication == null)
            {
                scoringApplication = ScoringApplication.Create(_clock.UtcNow);
                _scoringApplicationRepository.Add(scoringApplication);
            }

            if (!string.IsNullOrEmpty(saveScoringApplicationRequest.CountryCode))
            {
                var country = await _countryRepository.GetByCodeAsync(saveScoringApplicationRequest.CountryCode);
                if (country == null)
                    throw new AppErrorException(ErrorCode.CountryNotFound);

                scoringApplication.CountryId = country.Id;
            }
            else
            {
                scoringApplication.CountryId = null;
            }

            scoringApplication.ProjectId = project.Id;
            scoringApplication.ProjectName = saveScoringApplicationRequest.ProjectName;
            scoringApplication.Category = saveScoringApplicationRequest.ProjectCategory;
            scoringApplication.Stage = saveScoringApplicationRequest.ProjectStage;
            scoringApplication.ProjectDescription = saveScoringApplicationRequest.ProjectDescription;
            scoringApplication.Site = saveScoringApplicationRequest.Site;
            scoringApplication.WhitePaper = saveScoringApplicationRequest.WhitePaper;
            scoringApplication.IcoDate = saveScoringApplicationRequest.IcoDate;
            scoringApplication.ContactEmail = saveScoringApplicationRequest.ContactEmail;
            scoringApplication.SocialNetworks = SocialNetworkRequest.ToDomain(saveScoringApplicationRequest.SocialNetworks);
            scoringApplication.Articles = saveScoringApplicationRequest.Articles;
            scoringApplication.IsSubmitted = false;

            scoringApplication.Saved = _clock.UtcNow;

            var newTeamMembers = saveScoringApplicationRequest.TeamMembers.Select(x => new ScoringApplicationTeamMember
                                                                                       {
                                                                                           FullName = x.FullName,
                                                                                           ProjectRole = x.ProjectRole,
                                                                                           About = x.About,
                                                                                           FacebookLink = x.FacebookLink,
                                                                                           LinkedInLink = x.LinkedInLink,
                                                                                           AdditionalInformation = x.AdditionalInformation
                                                                                       }).ToList();

            scoringApplication.UpdateTeamMembers(newTeamMembers);

            var newAdvisers = saveScoringApplicationRequest.Advisers.Select(x => new ScoringApplicationAdviser
                                                                                 {
                                                                                     FullName = x.FullName,
                                                                                     About = x.About,
                                                                                     Reason = x.Reason,
                                                                                     FacebookLink = x.FacebookLink,
                                                                                     LinkedInLink = x.LinkedInLink
                                                                                 }).ToList();

            scoringApplication.UpdateAdvisers(newAdvisers);

            var scoringApplicationAnswers = saveScoringApplicationRequest.Answers.Select(ScoringApplicationAnswerRequest.ToDomain).ToList();
            scoringApplication.UpdateAnswers(scoringApplicationAnswers);

            await _scoringApplicationRepository.SaveChangesAsync();
        }

        public async Task SubmitApplicationAsync(long projectId)
        {
            var scoringApplication = await _scoringApplicationRepository.GetByProjectIdAsync(projectId);
            scoringApplication.IsSubmitted = true;
            scoringApplication.Submitted = _clock.UtcNow;

            await _scoringApplicationRepository.SaveChangesAsync();
        }
    }
}