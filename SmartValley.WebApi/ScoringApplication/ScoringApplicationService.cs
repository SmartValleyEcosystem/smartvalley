using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Domain;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.ScoringApplication.Requests;
using ScoringApplicationQuestion = SmartValley.Domain.Entities.ScoringApplicationQuestion;

namespace SmartValley.WebApi.ScoringApplication
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
            var country = await _countryRepository.GetByCodeAsync(saveScoringApplicationRequest.CountryCode);
            if (country == null)
                throw new AppErrorException(ErrorCode.CountryNotFound);

            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
                throw new AppErrorException(ErrorCode.ProjectNotFound);

            var scoringApplication = await _scoringApplicationRepository.GetByProjectIdAsync(projectId);
            if (scoringApplication == null)
            {
                scoringApplication = Domain.ScoringApplication.Create(_clock.UtcNow);
                _scoringApplicationRepository.Add(scoringApplication);
            }

            scoringApplication.ProjectId = project.Id;
            scoringApplication.ProjectName = saveScoringApplicationRequest.ProjectName;
            scoringApplication.Category = saveScoringApplicationRequest.Category;
            scoringApplication.Status = saveScoringApplicationRequest.Status;
            scoringApplication.ProjectDescription = saveScoringApplicationRequest.ProjectDescription;
            scoringApplication.CountryId = country.Id;
            scoringApplication.Site = saveScoringApplicationRequest.Site;
            scoringApplication.WhitePaper = saveScoringApplicationRequest.WhitePaper;
            scoringApplication.IcoDate = saveScoringApplicationRequest.IcoDate.ToString();
            scoringApplication.ContactEmail = saveScoringApplicationRequest.ContactEmail;
            scoringApplication.SocialNetworks = SocialNetworkRequest.ToDomain(saveScoringApplicationRequest.SocialNetworks);

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
            scoringApplication.UpdateAnswers(saveScoringApplicationRequest.Answers);

            await _scoringApplicationRepository.SaveChangesAsync();
        }

        public async Task SubmitForScoreAsync(long projectId)
        {
            var scoringApplication = await _scoringApplicationRepository.GetByProjectIdAsync(projectId);
            if (scoringApplication == null)
                throw new AppErrorException(ErrorCode.ProjectScoringApplicationNotFound);

            if (!scoringApplication.Submitted.HasValue)
            {
                scoringApplication.Submitted = _clock.UtcNow;
                await _scoringApplicationRepository.SaveChangesAsync();
            }
        }
    }
}