using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Application.Contracts.Scorings;
using SmartValley.Domain;
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
        private readonly IEstimateRepository _estimateRepository;
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
            IEstimateRepository estimateRepository,
            IScoringContractClient scoringContractClient,
            IScoringRepository scoringRepository,
            IClock clock,
            IScoringCriterionRepository scoringCriterionRepository,
            IScoringApplicationRepository scoringApplicationRepository)
        {
            _estimateRepository = estimateRepository;
            _scoringContractClient = scoringContractClient;
            _scoringRepository = scoringRepository;
            _clock = clock;
            _scoringCriterionRepository = scoringCriterionRepository;
            _scoringApplicationRepository = scoringApplicationRepository;
        }

        public async Task SubmitEstimatesAsync(long expertId, SubmitEstimatesRequest request)
        {
            var scoring = await _scoringRepository.GetByProjectIdAsync(request.ProjectId);
            var area = request.AreaType.ToDomain();

            if (!scoring.IsOfferAccepted(expertId, area))
                throw new AppErrorException(ErrorCode.AcceptedOfferNotFound);

            await AddEstimateCommentsAsync(expertId, request.EstimateComments, scoring.Id);

            var conclusion = new ExpertScoringConclusion
                             {
                                 ExpertId = expertId,
                                 Area = area,
                                 Conclusion = request.Conclusion
                             };
            scoring.AddConclusionForArea(conclusion);
            await UpdateProjectScoringAsync(scoring, area);
            scoring.FinishOffer(expertId, area);
            await _scoringRepository.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<ScoringStatisticsInArea>> GetScoringStatisticsAsync(long projectId)
        {
            var scoring = await _scoringRepository.GetByProjectIdAsync(projectId);
            if (scoring == null)
                return new ScoringStatisticsInArea[0];

            var estimates = (await GetEstimatesAsync(scoring.Id))
                .ToLookup(e => e.AreaType);

            return Enum.GetValues(typeof(AreaType)).Cast<AreaType>()
                       .Select(a => CreateScoringStatistics(a, scoring, estimates[a].ToArray()))
                       .ToArray();
        }

        private ScoringStatisticsInArea CreateScoringStatistics(AreaType areaType, Scoring scoring, IReadOnlyCollection<Estimate> estimates)
        {
            var areaScoring = scoring.GetAreaScoring(areaType);
            var conclusions = scoring.GetConclusionsForArea(areaType);
            return new ScoringStatisticsInArea(areaScoring.Score,
                                               areaScoring.ExpertsCount,
                                               estimates,
                                               conclusions,
                                               scoring.ScoringOffers
                                                      .Where(s => s.AreaId == areaType)
                                                      .ToArray(), areaType);
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
                                               Type = ScoringCriterionPromptType.SocialNetwork
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
                                       Type = ScoringCriterionPromptType.TeamMembers
                                   });
            }

            foreach (var id in _criterionsWithAdviserPrompts)
            {
                criteriaPromps.Add(new ScoringCriterionPrompt
                                   {
                                       CriterionId = id,
                                       Advisers = scoringApplication.Advisers.ToArray(),
                                       Type = ScoringCriterionPromptType.Advisers
                                   });
            }
        }

        private async Task<IReadOnlyCollection<Estimate>> GetEstimatesAsync(long scoringId)
        {
            var criteria = await _scoringCriterionRepository.GetAllAsync();
            var comments = await _estimateRepository.GetByScoringIdAsync(scoringId);

            return (from comment in comments
                    join criterion in criteria on comment.ScoringCriterionId equals criterion.Id
                    select new Estimate(comment.ScoringCriterionId, comment.Score.Value, comment.Comment, criterion.AreaType))
                .ToArray();
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

        private Task AddEstimateCommentsAsync(long expertId, IReadOnlyCollection<EstimateCommentRequest> estimateComments, long scoringId)
        {
            var estimates = estimateComments
                            .Select(e => CreateEstimateComment(e, scoringId, expertId))
                            .ToArray();

            return _estimateRepository.AddRangeAsync(estimates);
        }

        private static EstimateComment CreateEstimateComment(
            EstimateCommentRequest estimateComment,
            long scoringId,
            long expertId)
        {
            return new EstimateComment
                   {
                       ScoringId = scoringId,
                       ExpertId = expertId,
                       Score = estimateComment.Score,
                       ScoringCriterionId = estimateComment.ScoringCriterionId,
                       Comment = estimateComment.Comment
                   };
        }
    }
}