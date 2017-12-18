using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Application;
using SmartValley.Application.Contracts.Project;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.WebApi.Estimates
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EstimationService : IEstimationService
    {
        private readonly IEstimateCommentRepository _estimateCommentRepository;
        private readonly EthereumClient _ethereumClient;
        private readonly IProjectContractClient _projectContractClient;
        private readonly IProjectRepository _projectRepository;

        public EstimationService(
            IEstimateCommentRepository estimateCommentRepository,
            EthereumClient ethereumClient,
            IProjectContractClient projectContractClient,
            IProjectRepository projectRepository)
        {
            _estimateCommentRepository = estimateCommentRepository;
            _ethereumClient = ethereumClient;
            _projectContractClient = projectContractClient;
            _projectRepository = projectRepository;
        }

        public async Task SubmitEstimatesAsync(SubmitEstimatesRequest request)
        {
            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);

            await AddEstimateCommentsAsync(request);

            var project = await _projectRepository.GetByIdAsync(request.ProjectId);
            var scoringStatistics = await _projectContractClient.GetScoringStatisticsAsync(project.ProjectAddress);

            await UpdateProjectAsync(project, scoringStatistics);
        }

        public async Task<Dictionary<long, IReadOnlyCollection<Estimate>>> GetQuestionsWithEstimatesAsync(
            long projectId,
            string projectAddress,
            Domain.Entities.ExpertiseArea expertiseArea)
        {
            var estimateScores = await _projectContractClient.GetEstimatesAsync(projectAddress);
            var estimateComments = await _estimateCommentRepository.GetAsync(projectId, expertiseArea);

            return (from comment in estimateComments
                    join score in estimateScores
                        on new {comment.QuestionId, comment.ExpertAddress} equals new {score.QuestionId, score.ExpertAddress}
                    select CreateEstimate(projectId, score, comment))
                .GroupBy(e => e.QuestionId)
                .ToDictionary(g => g.Key, g => (IReadOnlyCollection<Estimate>) g.ToArray());
        }

        private static Estimate CreateEstimate(long projectId, EstimateScore score, EstimateComment comment)
        {
            return new Estimate
                   {
                       QuestionId = score.QuestionId,
                       Comment = comment.Comment,
                       ExpertAddress = score.ExpertAddress,
                       Score = score.Score,
                       ProjectId = projectId
                   };
        }

        private async Task UpdateProjectAsync(Project project, ProjectScoringStatistics scoringStatistics)
        {
            project.Score = scoringStatistics.Score;
            project.IsScoredByHr = scoringStatistics.IsScoredByHr;
            project.IsScoredByAnalyst = scoringStatistics.IsScoredByAnalyst;
            project.IsScoredByTechnical = scoringStatistics.IsScoredByTech;
            project.IsScoredByLawyer = scoringStatistics.IsScoredByLawyer;

            await _projectRepository.UpdateWholeAsync(project);
        }

        private Task AddEstimateCommentsAsync(SubmitEstimatesRequest request)
        {
            var estimates = request
                .EstimateComments
                .Select(e => CreateEstimateComment(e, request.ProjectId, request.ExpertAddress))
                .ToArray();

            return _estimateCommentRepository.AddRangeAsync(estimates);
        }

        private static EstimateComment CreateEstimateComment(
            EstimateCommentRequest estimateComment,
            long projectId,
            string expertAddress)
        {
            return new EstimateComment
                   {
                       ProjectId = projectId,
                       ExpertAddress = expertAddress,
                       QuestionId = estimateComment.QuestionId,
                       Comment = estimateComment.Comment
                   };
        }
    }
}