using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ProjectRepository : EntityCrudRepository<Project>, IProjectRepository
    {
        public ProjectRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
            : base(readContext, editContext)
        {
        }

        public async Task<IReadOnlyCollection<ProjectScoring>> GetAllScoredAsync()
        {
            return await (from project in ReadContext.Projects
                          join scoring in ReadContext.Scorings on project.Id equals scoring.ProjectId
                          where scoring.Score.HasValue
                          select new ProjectScoring(project, scoring)).ToArrayAsync();
        }

        public async Task<IReadOnlyCollection<ProjectScoring>> GetByAuthorAsync(string authorAddress)
        {
            return await (from project in ReadContext.Projects
                          from scoring in ReadContext.Scorings.Where(s => s.ProjectId == project.Id).DefaultIfEmpty()
                          where project.AuthorAddress.OrdinalEquals(authorAddress)
                          select new ProjectScoring(project, scoring)).ToArrayAsync();
        }

        public async Task<IReadOnlyCollection<Project>> GetForScoringAsync(string expertAddress, ExpertiseArea area)
        {
            var scoredProjects = (from question in ReadContext.Questions
                                  join comment in ReadContext.EstimateComments on question.Id equals comment.QuestionId
                                  join scoring in ReadContext.Scorings on comment.ProjectId equals scoring.ProjectId
                                  where (comment.ExpertAddress.OrdinalEquals(expertAddress) && question.ExpertiseArea == area) ||
                                        (area == ExpertiseArea.Analyst && scoring.IsScoredByAnalyst ||
                                         area == ExpertiseArea.Hr && scoring.IsScoredByHr ||
                                         area == ExpertiseArea.Lawyer && scoring.IsScoredByLawyer ||
                                         area == ExpertiseArea.Tech && scoring.IsScoredByTechnical)
                                  select comment.ProjectId).Distinct();

            return await ReadContext
                       .Projects
                       .Where(p => !scoredProjects.Contains(p.Id))
                       .ToArrayAsync();
        }

        public Task<Project> GetByExternalIdAsync(Guid externalId)
        {
            return ReadContext.Projects.FirstAsync(project => project.ExternalId == externalId);
        }
    }
}