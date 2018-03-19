using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Application.Contracts.Scorings;
using SmartValley.Domain;
using SmartValley.Domain.Core;
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

            var isOfferAccepted = await _scoringOffersRepository.IsAcceptedAsync(scoring.Id, expert.UserId, request.AreaType.ToDomain());
            if (!isOfferAccepted)
                throw new AppErrorException(ErrorCode.AcceptedOfferNotFound);

            await AddEstimateCommentsAsync(expert.UserId, request.EstimateComments, scoring.Id);

            await UpdateProjectScoringAsync(scoring);

            await _scoringOffersRepository.FinishAsync(scoring.Id, expert.UserId, request.AreaType.ToDomain());
        }

        public async Task<ScoringStatisticsInArea> GetScoringStatisticsInAreaAsync(long projectId, AreaType areaType)
        {
            var scoring = await _scoringRepository.GetByProjectIdAsync(projectId);
            if (scoring == null)
                return ScoringStatisticsInArea.Empty;

            var estimateScores = await _scoringContractClient.GetEstimatesAsync(scoring.ContractAddress);
            var comments = await _estimateCommentRepository.GetByProjectIdAsync(projectId, areaType);
            var users = await _userRepository.GetIdsByAddressesAsync(estimateScores.Select(x => x.ExpertAddress).ToArray());

            var estimates = (from user in users
                             join comment in comments
                                 on user.Id equals comment.ExpertId
                             join score in estimateScores
                                 on new {QuestionId = comment.QuestionId, Address = user.Address}
                                 equals new {QuestionId = score.QuestionId, Address = score.ExpertAddress}
                             select CreateEstimate(score, comment)).ToArray();

            var requiredSubmissionsInArea = (double) await _scoringContractClient.GetRequiredSubmissionsInAreaCountAsync(scoring.ContractAddress, areaType);
            var isCompletedInArea = await _scoringRepository.IsCompletedInAreaAsync(scoring.Id, areaType);
            var averageScore = isCompletedInArea ? estimates.Sum(i => i.Score) / requiredSubmissionsInArea : (double?) null;

            return new ScoringStatisticsInArea(averageScore, estimates);
        }

        private static Estimate CreateEstimate(EstimateScore score, EstimateComment comment)
            => new Estimate(score.QuestionId, score.Score, comment.Comment);

        private async Task UpdateProjectScoringAsync(Domain.Entities.Scoring scoring)
        {
            var scoringStatistics = await _scoringContractClient.GetScoringStatisticsAsync(scoring.ContractAddress);
            if (scoringStatistics.Score.HasValue)
            {
                scoring.Score = scoringStatistics.Score;
                scoring.ScoringEndDate = _clock.UtcNow;

                await _scoringRepository.UpdateWholeAsync(scoring);
            }

            await _scoringRepository.SetAreasCompletedAsync(scoring.Id, scoringStatistics.ScoredAreas);
        }

        private Task AddEstimateCommentsAsync(long expertUserId, IReadOnlyCollection<EstimateCommentRequest> estimateComments, long scoringId)
        {
            var estimates = estimateComments
                            .Select(e => CreateEstimateComment(e, scoringId, expertUserId))
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
                       QuestionId = estimateComment.QuestionId,
                       Comment = estimateComment.Comment
                   };
        }
    }
}