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
        private readonly IUserRepository _userRepository;
        private readonly IScoringCriterionRepository _scoringCriterionRepository;

        public EstimationService(
            IEstimateRepository estimateRepository,
            IScoringContractClient scoringContractClient,
            IScoringRepository scoringRepository,
            IClock clock,
            IUserRepository userRepository,
            IScoringCriterionRepository scoringCriterionRepository)
        {
            _estimateRepository = estimateRepository;
            _scoringContractClient = scoringContractClient;
            _scoringRepository = scoringRepository;
            _clock = clock;
            _userRepository = userRepository;
            _scoringCriterionRepository = scoringCriterionRepository;
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

            var estimates = (await GetEstimatesAsync(scoring.Id, scoring.ContractAddress))
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

        private async Task<IReadOnlyCollection<Estimate>> GetEstimatesAsync(long scoringId, string scoringContractAddress)
        {
            var criteria = await _scoringCriterionRepository.GetAllAsync();
            var estimateScores = await _scoringContractClient.GetEstimatesAsync(scoringContractAddress);
            var comments = await _estimateRepository.GetByScoringIdAsync(scoringId);
            var expertAddresses = estimateScores.Select(x => x.ExpertAddress).ToArray();
            var users = await _userRepository.GetByAddressesAsync(expertAddresses);

            return (from user in users
                    join comment in comments
                        on user.Id equals comment.ExpertId
                    join score in estimateScores
                        on new {ScoringCriterionId = comment.ScoringCriterionId, Address = user.Address}
                        equals new {ScoringCriterionId = score.ScoringCriterionId, Address = score.ExpertAddress}
                    join criterion in criteria on score.ScoringCriterionId equals criterion.Id
                    select new Estimate(score.ScoringCriterionId, score.Score, comment.Comment, criterion.AreaType))
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
                       ScoringCriterionId = estimateComment.ScoringCriterionId,
                       Comment = estimateComment.Comment
                   };
        }
    }
}