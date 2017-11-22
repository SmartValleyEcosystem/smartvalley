using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Domain.Interfaces;

namespace SmartValley.WebApi.Project
{
    [Route("api/project")]
    public class ProjectController : Controller
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        [HttpGet]
        [Route("scored")]
        public async Task<IReadOnlyCollection<ScoredProjectResponse>> GetAllScoredAsync()
        {
            var scoredProjects = await _projectRepository.GetAllScoredAsync();
            return scoredProjects.Select(CreateResponse).ToArray();
        }

        private static ScoredProjectResponse CreateResponse(Domain.Entities.Project project)
        {
            return new ScoredProjectResponse
                   {
                       Id = project.Id,
                       Name = project.Name,
                       Address = project.ProjectAddress,
                       Country = project.Country,
                       Area = project.ProjectArea,
                       Description = project.ProblemDesc,
                       Score = project.Score.Value
                   };
        }
    }
}