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
        private readonly IScoringOffersRepository _scoringOffersRepository;
        private readonly IClock _clock;
        private readonly IUserRepository _userRepository;
        private readonly IScoringCriterionRepository _scoringCriterionRepository;
        private readonly IScoringApplicationRepository _scoringApplicationRepository;

        private IReadOnlyCollection<long> _criterionsWithTeamMemberPrompts = new List<long>
                                                                             {
                                                                                 14,
                                                                                 18
                                                                             };

        private IReadOnlyCollection<long> _criterionsWithAdviserPrompts = new List<long>
                                                                          {
                                                                              15,
                                                                              16,
                                                                              17
                                                                          };

        private IReadOnlyCollection<long> _criterionsWithSocialNetworksPrompts = new List<long>
                                                                                 {
                                                                                     49
                                                                                 };

        public EstimationService(
            IEstimateRepository estimateRepository,
            IScoringContractClient scoringContractClient,
            IScoringRepository scoringRepository,
            IScoringOffersRepository scoringOffersRepository,
            IClock clock,
            IUserRepository userRepository,
            IScoringCriterionRepository scoringCriterionRepository,
            IScoringApplicationRepository scoringApplicationRepository)
        {
            _estimateRepository = estimateRepository;
            _scoringContractClient = scoringContractClient;
            _scoringRepository = scoringRepository;
            _scoringOffersRepository = scoringOffersRepository;
            _clock = clock;
            _userRepository = userRepository;
            _scoringCriterionRepository = scoringCriterionRepository;
            _scoringApplicationRepository = scoringApplicationRepository;
        }

        public async Task SubmitEstimatesAsync(long expertId, SubmitEstimatesRequest request)
        {
            var scoring = await _scoringRepository.GetByProjectIdAsync(request.ProjectId);

            var area = request.AreaType.ToDomain();
            if (!await _scoringOffersRepository.IsAcceptedAsync(scoring.Id, expertId, area))
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
            await _scoringRepository.SaveChangesAsync();

            await _scoringOffersRepository.FinishAsync(scoring.Id, expertId, area);
        }

        public async Task<ScoringStatisticsInArea> GetScoringStatisticsInAreaAsync(long projectId, AreaType areaType)
        {
            var scoring = await _scoringRepository.GetByProjectIdAsync(projectId);
            if (scoring == null)
                return ScoringStatisticsInArea.Empty;

            var estimates = await GetEstimatesAsync(scoring.Id, scoring.ContractAddress, areaType);
            var areaScore = scoring.GetScoreForArea(areaType);
            var conclusions = scoring.GetConclusionsForArea(areaType);
            return new ScoringStatisticsInArea(areaScore, estimates, conclusions, scoring.ScoringOffers.ToArray());
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

        private async Task<IReadOnlyCollection<Estimate>> GetEstimatesAsync(long scoringId, string scoringContractAddress, AreaType areaType)
        {
            var estimateScores = await _scoringContractClient.GetEstimatesAsync(scoringContractAddress);
            var comments = await _estimateRepository.GetByScoringIdAsync(scoringId, areaType);
            var expertAddresses = estimateScores.Select(x => x.ExpertAddress).ToArray();
            var users = await _userRepository.GetByAddressesAsync(expertAddresses);

            return (from user in users
                    join comment in comments
                        on user.Id equals comment.ExpertId
                    join score in estimateScores
                        on new {ScoringCriterionId = comment.ScoringCriterionId, Address = user.Address}
                        equals new {ScoringCriterionId = score.ScoringCriterionId, Address = score.ExpertAddress}
                    select CreateEstimate(score, comment))
                .ToArray();
        }

        private static Estimate CreateEstimate(EstimateScore score, EstimateComment comment)
            => new Estimate(score.ScoringCriterionId, score.Score, comment.Comment);

        private async Task UpdateProjectScoringAsync(Domain.Entities.Scoring scoring, AreaType area)
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
                       ScoringCriterionId = estimateComment.ScoringCriterionId,
                       Comment = estimateComment.Comment
                   };
        }
    }
}