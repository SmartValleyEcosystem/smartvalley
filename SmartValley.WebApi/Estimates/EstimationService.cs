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
        private readonly IEstimateCommentRepository _estimateCommentRepository;
        private readonly IScoringContractClient _scoringContractClient;
        private readonly IScoringRepository _scoringRepository;
        private readonly IScoringOffersRepository _scoringOffersRepository;
        private readonly IExpertRepository _expertRepository;
        private readonly IClock _clock;
        private readonly IUserRepository _userRepository;

        public EstimationService(
            IEstimateCommentRepository estimateCommentRepository,
            IScoringContractClient scoringContractClient,
            IScoringRepository scoringRepository,
            IScoringOffersRepository scoringOffersRepository,
            IExpertRepository expertRepository,
            IClock clock,
            IUserRepository userRepository)
        {
            _estimateCommentRepository = estimateCommentRepository;
            _scoringContractClient = scoringContractClient;
            _scoringRepository = scoringRepository;
            _scoringOffersRepository = scoringOffersRepository;
            _expertRepository = expertRepository;
            _clock = clock;
            _userRepository = userRepository;
        }

        public async Task SubmitEstimatesAsync(SubmitEstimatesRequest request)
        {
            var scoring = await _scoringRepository.GetByProjectIdAsync(request.ProjectId);
            var expert = await _expertRepository.GetByAddressAsync(request.ExpertAddress);

            var area = request.AreaType.ToDomain();
            var isOfferAccepted = await _scoringOffersRepository.IsAcceptedAsync(scoring.Id, expert.UserId, area);
            if (!isOfferAccepted)
                throw new AppErrorException(ErrorCode.AcceptedOfferNotFound);

            await AddEstimateCommentsAsync(expert.UserId, request.EstimateComments, scoring.Id);

            await UpdateProjectScoringAsync(scoring, area);

            await _scoringOffersRepository.FinishAsync(scoring.Id, expert.UserId, area);
        }

        public async Task<ScoringStatisticsInArea> GetScoringStatisticsInAreaAsync(long projectId, AreaType areaType)
        {
            var scoring = await _scoringRepository.GetByProjectIdAsync(projectId);
            if (scoring == null)
                return ScoringStatisticsInArea.Empty;

            var estimates = await GetEstimatesAsync(scoring.Id, scoring.ContractAddress, areaType);
            var areaScore = await _scoringRepository.GetAreaScoreAsync(scoring.Id, areaType);
            return new ScoringStatisticsInArea(areaScore, estimates);
        }

        private async Task<IReadOnlyCollection<Estimate>> GetEstimatesAsync(long scoringId, string scoringContractAddress, AreaType areaType)
        {
            var estimateScores = await _scoringContractClient.GetEstimatesAsync(scoringContractAddress);
            var comments = await _estimateCommentRepository.GetByScoringIdAsync(scoringId, areaType);
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

                await _scoringRepository.UpdateWholeAsync(scoring);
            }

            var areaScore = scoringStatistics.AreaScores[area];
            if (areaScore.HasValue)
                await _scoringRepository.SetAreaScoreAsync(scoring.Id, area, areaScore.Value);
        }

        private Task AddEstimateCommentsAsync(long expertId, IReadOnlyCollection<EstimateCommentRequest> estimateComments, long scoringId)
        {
            var estimates = estimateComments
                            .Select(e => CreateEstimateComment(e, scoringId, expertId))
                            .ToArray();

            return _estimateCommentRepository.AddRangeAsync(estimates);
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