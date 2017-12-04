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
        private const byte RequiredEstimatesCountInCategory = 3;

        private readonly IEstimateRepository _estimateRepository;
        private readonly IProjectRepository _projectRepository;

        public EstimationService(
            IEstimateRepository estimateRepository,
            IProjectRepository projectRepository)
        {
            _estimateRepository = estimateRepository;
            _projectRepository = projectRepository;
        }

        public async Task SubmitEstimatesAsync(SubmitEstimatesRequest request)
        {
            var expertiseArea = request.ExpertiseArea.ToDomain();
            var project = await _projectRepository.GetByIdAsync(request.ProjectId);

            var currentCounterVaule = project.GetEstimatesCounterValue(expertiseArea);
            if (currentCounterVaule == RequiredEstimatesCountInCategory)
            {
                throw new AppErrorException(
                    ErrorCode.ProjectAlreadyEstimatedInCategory,
                    $"Project '{project.Name}' already received {RequiredEstimatesCountInCategory} estimates in category '{expertiseArea}'.");
            }

            var projectsEstimatedByExpert = await _estimateRepository.GetProjectsEstimatedByExpertAsync(request.ExpertAddress, expertiseArea);
            if (projectsEstimatedByExpert.Contains(project.Id))
            {
                throw new AppErrorException(
                    ErrorCode.ExpertAlreadyEstimatedProjectInCategory,
                    $"Project '{project.Name}' has already been estimated in category '{expertiseArea}' by expert '{request.ExpertAddress}'.");
            }

            await AddEstimatesAsync(request, expertiseArea);

            await UpdateProjectAsync(project, expertiseArea);
        }

        public Task<IReadOnlyCollection<Estimate>> GetAsync(long projectId, ExpertiseAreaApi expertiseAreaApi)
        {
            return _estimateRepository.GetAsync(projectId, expertiseAreaApi.ToDomain());
        }

        public double CalculateAverageScore(IReadOnlyCollection<Estimate> estimates)
        {
            var expertsCount = estimates.GroupBy(e => e.ExpertAddress).Count();
            if (expertsCount < RequiredEstimatesCountInCategory)
            {
                return -1;
            }
            return estimates.Sum(e => e.Score) / (double) RequiredEstimatesCountInCategory;
        }

        private async Task UpdateProjectAsync(Project project, ExpertiseArea expertiseArea)
        {
            project.IncrementEstimatesCounter(expertiseArea);

            if (project.IsReadyForScoring(RequiredEstimatesCountInCategory))
                project.Score = await CalculateProjectScoreAsync(project.Id);

            await _projectRepository.UpdateWholeAsync(project);
        }

        private async Task<double> CalculateProjectScoreAsync(long projectId)
        {
            var estimates = await _estimateRepository.GetAsync(projectId);
            return (double) estimates.Sum(e => e.Score) / RequiredEstimatesCountInCategory;
        }

        private Task AddEstimatesAsync(SubmitEstimatesRequest request, ExpertiseArea expertiseArea)
        {
            var estimates = request
                .Estimates
                .Select(e => CreateEstimate(e, request.ProjectId, request.ExpertAddress, expertiseArea))
                .ToArray();

            return _estimateRepository.AddRangeAsync(estimates);
        }

        private static Estimate CreateEstimate(
            EstimateRequest estimateRequest,
            long projectId,
            string expertAddress,
            ExpertiseArea category)
        {
            return new Estimate
                   {
                       ProjectId = projectId,
                       ExpertAddress = expertAddress,
                       QuestionIndex = estimateRequest.QuestionId,
                       Score = estimateRequest.Score,
                       ScoringCategory = category,
                       Comment = estimateRequest.Comment
                   };
        }
    }
}