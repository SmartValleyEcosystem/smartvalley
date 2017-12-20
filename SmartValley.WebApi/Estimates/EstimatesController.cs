using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Application.Contracts.Project;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Projects;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Estimates
{
    [Route("api/estimates")]
    public class EstimatesController : Controller
    {
        private readonly IEstimationService _estimationService;
        private readonly IProjectService _projectService;
        private readonly IQuestionRepository _questionRepository;
        private readonly IProjectContractClient _projectContractClient;

        public EstimatesController(
            IEstimationService estimationService,
            IProjectService projectService,
            IQuestionRepository questionRepository,
            IProjectContractClient projectContractClient)
        {
            _estimationService = estimationService;
            _projectService = projectService;
            _questionRepository = questionRepository;
            _projectContractClient = projectContractClient;
        }

        [Authorize]
        public async Task<IActionResult> Post([FromBody] SubmitEstimatesRequest request)
        {
            await _estimationService.SubmitEstimatesAsync(request);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetEstimatesAsync(GetEstimatesRequest request)
        {
            var project = await _projectService.GetProjectByIdAsync(request.ProjectId);
            var scoringStatistics = await _projectContractClient.GetScoringStatisticsAsync(project.ProjectAddress);
            if (scoringStatistics.Score == null && !project.AuthorAddress.Equals(User.Identity.Name, StringComparison.InvariantCultureIgnoreCase))
                return Unauthorized();

            var expertiseArea = request.ExpertiseArea.ToDomain();
            var isScoredInArea = scoringStatistics.IsScoredInArea(expertiseArea);
            var questionsWithEstimates = await _estimationService.GetQuestionsWithEstimatesAsync(request.ProjectId, project.ProjectAddress, expertiseArea);

            var response = CreateGetQuestionsWithEstimatesResponse(isScoredInArea, questionsWithEstimates);
            return Ok(response);
        }

        private static GetQuestionsWithEstimatesResponse CreateGetQuestionsWithEstimatesResponse(
            bool isProjectScoredInArea,
            Dictionary<long, IReadOnlyCollection<Estimate>> questionsWithEstimates)
        {
            var averageScore = isProjectScoredInArea
                                   ? questionsWithEstimates.Values.SelectMany(s => s.Select(i => i.Score)).Sum() / 3
                                   : (double?) null;

            return new GetQuestionsWithEstimatesResponse
                   {
                       AverageScore = averageScore,
                       Questions = questionsWithEstimates
                           .Select(q => CreateQuestionWithEstimatesResponse(q.Key, q.Value))
                           .ToArray()
                   };
        }

        private static QuestionWithEstimatesResponse CreateQuestionWithEstimatesResponse(long questionId, IReadOnlyCollection<Estimate> estimates)
        {
            return new QuestionWithEstimatesResponse
                   {
                       QuestionId = questionId,
                       Estimates = estimates.Select(EstimateResponse.Create).ToArray()
                   };
        }

        [HttpGet]
        [Route("questions")]
        public async Task<CollectionResponse<QuestionResponse>> GetQuestionsAsync()
        {
            var questions = await _questionRepository.GetAllAsync();
            return new CollectionResponse<QuestionResponse>
                   {
                       Items = questions.Select(QuestionResponse.From).ToArray()
                   };
        }
    }
}