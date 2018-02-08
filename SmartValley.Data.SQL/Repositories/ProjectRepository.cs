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

        public async Task<IReadOnlyCollection<ProjectScoring>> GetForScoringAsync(string expertAddress, ExpertiseAreaType areaType)
        {
            var scoredProjects = (from question in ReadContext.Questions
                                  join comment in ReadContext.EstimateComments on question.Id equals comment.QuestionId
                                  join scoring in ReadContext.Scorings on comment.ProjectId equals scoring.ProjectId
                                  where (comment.ExpertAddress.OrdinalEquals(expertAddress) && question.ExpertiseAreaType == areaType) ||
                                        (areaType == ExpertiseAreaType.Analyst && scoring.IsScoredByAnalyst ||
                                         areaType == ExpertiseAreaType.Hr && scoring.IsScoredByHr ||
                                         areaType == ExpertiseAreaType.Lawyer && scoring.IsScoredByLawyer ||
                                         areaType == ExpertiseAreaType.Tech && scoring.IsScoredByTechnical)
                                  select comment.ProjectId).Distinct();

            return await (from project in ReadContext.Projects
                          where !scoredProjects.Contains(project.Id)
                          join scoring in ReadContext.Scorings on project.Id equals scoring.ProjectId
                          select new ProjectScoring(project, scoring)).ToArrayAsync();
        }

        public Task<Project> GetByExternalIdAsync(Guid externalId)
        {
            return ReadContext.Projects.FirstAsync(project => project.ExternalId == externalId);
        }

        public async Task<IReadOnlyCollection<Project>> GetByExternalIdsAsync(IReadOnlyCollection<Guid> externalIds)
        {
            return await ReadContext.Projects.Where(project => externalIds.Contains(project.ExternalId)).ToArrayAsync();
        }
    }
}