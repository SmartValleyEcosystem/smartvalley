using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Application.Exceptions;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.WebApi.Estimates
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EstimationService : IEstimationService
    {
        private const byte RequiredEstimatesCountInExpertiseArea = 3;

        private readonly IEstimateRepository _estimateRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IProjectRepository _projectRepository;

        public EstimationService(
            IEstimateRepository estimateRepository,
            IProjectRepository projectRepository,
            IQuestionRepository questionRepository)
        {
            _estimateRepository = estimateRepository;
            _projectRepository = projectRepository;
            _questionRepository = questionRepository;
        }

        public async Task SubmitEstimatesAsync(SubmitEstimatesRequest request)
        {
            var expertiseArea = request.ExpertiseArea.ToDomain();
            var project = await _projectRepository.GetByIdAsync(request.ProjectId);

            var currentCounterVaule = project.GetEstimatesCounterValue(expertiseArea);
            if (currentCounterVaule == RequiredEstimatesCountInExpertiseArea)
            {
                throw new AppErrorException(
                    ErrorCode.ProjectAlreadyEstimatedInExpertiseArea,
                    $"Project '{project.Name}' already received {RequiredEstimatesCountInExpertiseArea} estimates in area '{expertiseArea}'.");
            }

            var projectsEstimatedByExpert = await _estimateRepository.GetProjectsEstimatedByExpertAsync(request.ExpertAddress, expertiseArea);
            if (projectsEstimatedByExpert.Contains(project.Id))
            {
                throw new AppErrorException(
                    ErrorCode.ExpertAlreadyEstimatedProjectInExpertiseArea,
                    $"Project '{project.Name}' has already been estimated in area '{expertiseArea}' by expert '{request.ExpertAddress}'.");
            }

            await AddEstimatesAsync(request);

            await UpdateProjectAsync(project, expertiseArea);
        }

        public Task<Dictionary<long, IReadOnlyCollection<Estimate>>> GetQuestionWithEstimatesAsync(long projectId, ExpertiseArea expertiseArea)
        {
            return _questionRepository.GetQuestionWithEstimatesAsync(projectId, expertiseArea.ToDomain());
        }

        public double CalculateAverageScore(IReadOnlyCollection<Estimate> estimates)
        {
            var expertsCount = estimates.GroupBy(e => e.ExpertAddress).Count();
            if (expertsCount < RequiredEstimatesCountInExpertiseArea)
            {
                return -1;
            }
            return estimates.Sum(e => e.Score) / (double) RequiredEstimatesCountInExpertiseArea;
        }

        private async Task UpdateProjectAsync(Project project, Domain.Entities.ExpertiseArea expertiseArea)
        {
            project.IncrementEstimatesCounter(expertiseArea);

            if (project.IsReadyForScoring(RequiredEstimatesCountInExpertiseArea))
                project.Score = await CalculateProjectScoreAsync(project.Id);

            await _projectRepository.UpdateWholeAsync(project);
        }

        private async Task<double> CalculateProjectScoreAsync(long projectId)
        {
            var estimates = await _estimateRepository.GetAsync(projectId);
            return (double) estimates.Sum(e => e.Score) / RequiredEstimatesCountInExpertiseArea;
        }

        private Task AddEstimatesAsync(SubmitEstimatesRequest request)
        {
            var estimates = request
                .Estimates
                .Select(e => CreateEstimate(e, request.ProjectId, request.ExpertAddress))
                .ToArray();

            return _estimateRepository.AddRangeAsync(estimates);
        }

        private static Estimate CreateEstimate(
            EstimateRequest estimateRequest,
            long projectId,
            string expertAddress)
        {
            return new Estimate
                   {
                       ProjectId = projectId,
                       ExpertAddress = expertAddress,
                       QuestionId = estimateRequest.QuestionId,
                       Score = estimateRequest.Score,
                       Comment = estimateRequest.Comment
                   };
        }
    }
}