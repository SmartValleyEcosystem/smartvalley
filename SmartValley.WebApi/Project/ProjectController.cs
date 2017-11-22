using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Domain.Interfaces;

namespace SmartValley.WebApi.Project
{
    [Route("api/project")]
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        [HttpGet]
        [Route("scored")]
        public async Task<IReadOnlyCollection<ScoredProjectSummaryResponse>> GetAllScoredAsync()
        {
            var scoredProjects = await _projectRepository.GetAllScoredAsync();
            return scoredProjects.Select(CreateResponse).ToArray();
        }

        private static ScoredProjectSummaryResponse CreateResponse(Domain.Entities.Project project)
        {
            return new ScoredProjectSummaryResponse
                   {
                       Id = project.Id,
                       Name = project.Name,
                       Country = project.Country,
                       ProjectArea = project.ProjectArea,
                       ProblemDescription = project.ProblemDesc,
                       SolutionDescription = project.SolutionDesc,
                       Score = project.Score.Value
                   };
        }
    }
}