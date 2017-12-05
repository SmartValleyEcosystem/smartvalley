using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public EstimatesController(IEstimationService estimationService, IProjectService projectService, IQuestionRepository questionRepository)
        {
            _estimationService = estimationService;
            _projectService = projectService;
            _questionRepository = questionRepository;
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

            if (project.Score == null && !project.AuthorAddress.Equals(User.Identity.Name, StringComparison.InvariantCultureIgnoreCase))
                return Unauthorized();

            var questionsWithEstimates = await _estimationService.GetQuestionWithEstimatesAsync(request.ProjectId, request.ExpertiseArea);

            var averageScore = _estimationService.CalculateAverageScore(questionsWithEstimates.Values.SelectMany(s => s).ToArray());

            var response = new GetQuestionsWithEstimatesResponse
                           {
                               AverageScore = averageScore,
                               Questions = questionsWithEstimates.Select(q => new QuestionWithEstimatesResponse
                                                                              {
                                                                                  QuestionId = q.Key,
                                                                                  Estimates = q.Value.Select(EstimateResponse.From).ToArray()
                                                                              })
                                                                 .ToArray()
                           };

            return Ok(response);
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