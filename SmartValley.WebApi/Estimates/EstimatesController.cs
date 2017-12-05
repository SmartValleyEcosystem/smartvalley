using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.Projects;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Estimates
{
    [Route("api/estimates")]
    public class EstimatesController : Controller
    {
        private readonly IEstimationService _estimationService;
        private readonly IProjectService _projectService;

        public EstimatesController(IEstimationService estimationService, IProjectService projectService)
        {
            _estimationService = estimationService;
            _projectService = projectService;
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

            var estimates = await _estimationService.GetAsync(request.ProjectId, request.ExpertiseArea);
            var averageScore = _estimationService.CalculateAverageScore(estimates);

            var response = new GetQuestionsWithEstimatesResponse
                           {
                               AverageScore = averageScore,
                               Questions = new List<QuestionWithEstimatesResponse>
                                           {
                                               new QuestionWithEstimatesResponse
                                               {
                                                   QuestionId = 1,
                                                   Estimates = new List<EstimateResponse>
                                                               {
                                                                   new EstimateResponse
                                                                   {
                                                                       Score = 5,
                                                                       Comment = "test"
                                                                   },
                                                                   new EstimateResponse
                                                                   {
                                                                       Score = 3,
                                                                       Comment = "test2"
                                                                   }
                                                               }
                                               }
                                           }
                           };

            return Ok(response);
        }

        [HttpGet]
        [Route("questions")]
        public async Task<CollectionResponse<QuestionResponse>> GetQuestionsAsync()
        {
            return new CollectionResponse<QuestionResponse>
                   {
                       Items = new List<QuestionResponse>
                               {
                                   new QuestionResponse
                                   {
                                       Id = 1,
                                       Name = "name1",
                                       Description = "desc1",
                                       ExpertiseArea = ExpertiseAreaApi.Hr,
                                       MinScore = 0,
                                       MaxScore = 10
                                   },
                                   new QuestionResponse
                                   {
                                       Id = 2,
                                       Name = "name2",
                                       Description = "desc2",
                                       ExpertiseArea = ExpertiseAreaApi.Lawyer,
                                       MinScore = 0,
                                       MaxScore = 10
                                   },
                                   new QuestionResponse
                                   {
                                       Id = 3,
                                       Name = "name3",
                                       Description = "desc3",
                                       ExpertiseArea = ExpertiseAreaApi.Analyst,
                                       MinScore = 0,
                                       MaxScore = 10
                                   }
                               }
                   };
        }
    }
}