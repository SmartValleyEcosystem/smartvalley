using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Domain;
using SmartValley.Domain.Contracts;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Estimates.Requests;
using SmartValley.WebApi.Experts;
using AreaType = SmartValley.Domain.Entities.AreaType;

namespace SmartValley.WebApi.Estimates
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EstimationService : IEstimationService
    {
        private readonly IScoringContractClient _scoringContractClient;
        private readonly IScoringRepository _scoringRepository;
        private readonly IClock _clock;
        private readonly IScoringCriterionRepository _scoringCriterionRepository;
        private readonly IScoringApplicationRepository _scoringApplicationRepository;

        private readonly IReadOnlyCollection<long> _criterionsWithTeamMemberPrompts = new List<long>
                                                                                      {
                                                                                          14,
                                                                                          18
                                                                                      };

        private readonly IReadOnlyCollection<long> _criterionsWithAdviserPrompts = new List<long>
                                                                                   {
                                                                                       15,
                                                                                       16,
                                                                                       17
                                                                                   };

        private readonly IReadOnlyCollection<long> _criterionsWithSocialNetworksPrompts = new List<long>
                                                                                          {
                                                                                              49
                                                                                          };

        public EstimationService(
            IScoringContractClient scoringContractClient,
            IScoringRepository scoringRepository,
            IClock clock,
            IScoringCriterionRepository scoringCriterionRepository,
            IScoringApplicationRepository scoringApplicationRepository)
        {
            _scoringContractClient = scoringContractClient;
            _scoringRepository = scoringRepository;
            _clock = clock;
            _scoringCriterionRepository = scoringCriterionRepository;
            _scoringApplicationRepository = scoringApplicationRepository;
        }

        public async Task SaveEstimatesAsync(long expertId, SaveEstimatesRequest request)
        {
            var scoring = await _scoringRepository.GetByProjectIdAsync(request.ProjectId);
            var area = request.AreaType.ToDomain();

            var estimates = request.EstimateComments.Select(x => new Estimate
                                                                 {
                                                                     Score = x.Score,
                                                                     ScoringCriterionId = x.ScoringCriterionId,
                                                                     Comment = x.Comment
                                                                 }).ToArray();

            var expertScoring = new ExpertScoring
                                {
                                    ExpertId = expertId,
                                    Area = area,
                                    Conclusion = request.Conclusion,
                                    Estimates = estimates
                                };
            scoring.SetExpertScoring(expertId, expertScoring);
            await _scoringRepository.SaveChangesAsync();
        }

        public async Task<ExpertScoring> GetOfferEstimateAsync(long expertId, long projectId, Experts.AreaType areaType)
        {
            var scoring = await _scoringRepository.GetByProjectIdAsync(projectId);
            return scoring.ExpertScorings.FirstOrDefault(x => x.ExpertId == expertId && x.Area == areaType.ToDomain());
        }

        public async Task SubmitEstimatesAsync(long expertId, SubmitEstimateRequest request)
        {
            var scoring = await _scoringRepository.GetByProjectIdAsync(request.ProjectId);
            var area = request.AreaType.ToDomain();

            if (!scoring.IsOfferAccepted(expertId, area))
                throw new AppErrorException(ErrorCode.AcceptedOfferNotFound);

            await UpdateProjectScoringAsync(scoring, area);
            scoring.FinishOffer(expertId, area);
            await _scoringRepository.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<ScoringStatisticsInArea>> GetScoringStatisticsAsync(long projectId)
        {
            var scoring = await _scoringRepository.GetByProjectIdAsync(projectId);
            if (scoring == null)
                return new ScoringStatisticsInArea[0];

            var result = new List<ScoringStatisticsInArea>();
            foreach (var areaScoring in scoring.AreaScorings)
            {
                var expertScorings = scoring.ExpertScorings.Where(x => x.Area == areaScoring.AreaId).ToArray();
                var offers = scoring.ScoringOffers.Where(s => s.AreaId == areaScoring.AreaId).ToArray();
                var statistics = new ScoringStatisticsInArea(areaScoring.Score,
                                                             areaScoring.ExpertsCount,
                                                             expertScorings, offers, areaScoring.AreaId);
                result.Add(statistics);
            }

            return result;
        }

        public async Task<IReadOnlyCollection<ScoringCriterionPrompt>> GetCriterionPromptsAsync(long projectId, AreaType areaType)
        {
            var scoringApplication = await _scoringApplicationRepository.GetByProjectIdAsync(projectId);
            var criteriaPromps = await _scoringCriterionRepository.GetScoringCriterionPromptsAsync(scoringApplication.Id, areaType);

            switch (areaType)
            {
                case AreaType.Hr:
                    await AddHrCriterionPromptExceptions(scoringApplication.Id, criteriaPromps);
                    break;
                case AreaType.Marketer:
                    var socialNetworks = (await _scoringApplicationRepository.GetByIdAsync(scoringApplication.Id)).SocialNetworks;

                    foreach (var id in _criterionsWithSocialNetworksPrompts)
                    {
                        criteriaPromps.Add(new ScoringCriterionPrompt
                                           {
                                               CriterionId = id,
                                               SocialNetworks = socialNetworks,
                                               PromptType = ScoringCriterionPromptType.SocialNetwork
                                           });
                    }

                    break;
            }

            return criteriaPromps.ToArray();
        }

        private async Task AddHrCriterionPromptExceptions(long scoringApplicationId, IList<ScoringCriterionPrompt> criteriaPromps)
        {
            var scoringApplication = await _scoringApplicationRepository.GetByIdAsync(scoringApplicationId);

            foreach (var id in _criterionsWithTeamMemberPrompts)
            {
                criteriaPromps.Add(new ScoringCriterionPrompt
                                   {
                                       CriterionId = id,
                                       TeamMembers = scoringApplication.TeamMembers.ToArray(),
                                       PromptType = ScoringCriterionPromptType.TeamMembers
                                   });
            }

            foreach (var id in _criterionsWithAdviserPrompts)
            {
                criteriaPromps.Add(new ScoringCriterionPrompt
                                   {
                                       CriterionId = id,
                                       Advisers = scoringApplication.Advisers.ToArray(),
                                       PromptType = ScoringCriterionPromptType.Advisers
                                   });
            }
        }

        private async Task UpdateProjectScoringAsync(Scoring scoring, AreaType area)
        {
            var scoringStatistics = await _scoringContractClient.GetScoringStatisticsAsync(scoring.ContractAddress);
            if (scoringStatistics.Score.HasValue)
            {
                scoring.Score = scoringStatistics.Score;
                scoring.ScoringEndDate = _clock.UtcNow;
                scoring.Status = ScoringStatus.Finished;
            }

            var areaScore = scoringStatistics.AreaScores[area];
            if (areaScore.HasValue)
            {
                scoring.SetScoreForArea(area, areaScore.Value);
            }
        }
    }
}