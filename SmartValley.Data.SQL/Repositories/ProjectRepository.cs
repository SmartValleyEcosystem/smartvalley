using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain;
using SmartValley.Domain.Core;
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

        public async Task<IReadOnlyCollection<ProjectDetails>> GetScoredAsync(int page, int pageSize)
        {
            return await (from project in ReadContext.Projects
                          join scoring in ReadContext.Scorings on project.Id equals scoring.ProjectId
                          join application in ReadContext.Applications on project.Id equals application.ProjectId
                          join country in ReadContext.Countries on project.CountryId equals country.Id
                          where scoring.Score.HasValue
                          orderby scoring.ScoringEndDate descending
                          select new ProjectDetails(project, scoring, application, country))
                       .Skip(page * pageSize)
                       .Take(pageSize)
                       .ToArrayAsync();
        }

        public async Task<IReadOnlyCollection<ProjectDetails>> GetByAuthorAsync(Address authorAddress)
        {
            return await (from project in ReadContext.Projects
                          from scoring in ReadContext.Scorings.Where(s => s.ProjectId == project.Id).DefaultIfEmpty()
                          join application in ReadContext.Applications on project.Id equals application.ProjectId
                          join country in ReadContext.Countries on project.CountryId equals country.Id
                          where project.AuthorAddress == authorAddress
                          select new ProjectDetails(project, scoring, application, country)).ToArrayAsync();
        }

        public async Task<IReadOnlyCollection<ProjectDetails>> GetForScoringAsync(AreaType areaType, long expertId)
        {
            var scoredProjects = (from question in ReadContext.Questions
                                  join comment in ReadContext.EstimateComments on question.Id equals comment.QuestionId
                                  join scoring in ReadContext.Scorings on comment.ProjectId equals scoring.ProjectId
                                  join areaScoring in ReadContext.AreaScorings on scoring.Id equals areaScoring.ScoringId
                                  where comment.ExpertId == expertId && question.AreaType == areaType ||
                                        areaScoring.AreaId == areaType && areaScoring.IsCompleted
                                  select comment.ProjectId).Distinct();

            return await (from project in ReadContext.Projects
                          where !scoredProjects.Contains(project.Id)
                          join scoring in ReadContext.Scorings on project.Id equals scoring.ProjectId
                          join application in ReadContext.Applications on project.Id equals application.ProjectId
                          join country in ReadContext.Countries on project.CountryId equals country.Id
                          select new ProjectDetails(project, scoring, application, country)).ToArrayAsync();
        }

        public Task<Project> GetByExternalIdAsync(Guid externalId)
            => ReadContext.Projects.FirstAsync(project => project.ExternalId == externalId);

        public async Task<IReadOnlyCollection<ProjectDetails>> GetByExternalIdsAsync(IReadOnlyCollection<Guid> externalIds)
        {
            return await (from project in ReadContext.Projects
                          join scoring in ReadContext.Scorings on project.Id equals scoring.ProjectId
                          join application in ReadContext.Applications on project.Id equals application.ProjectId
                          join country in ReadContext.Countries on project.CountryId equals country.Id
                          where externalIds.Contains(project.ExternalId)
                          select new ProjectDetails(project, scoring, application, country)).ToArrayAsync();
        }

        public async Task<IReadOnlyCollection<ProjectDetails>> GetAllByNameAsync(string projectName)
        {
            return await (from project in ReadContext.Projects
                          join scoring in ReadContext.Scorings on project.Id equals scoring.ProjectId
                          join application in ReadContext.Applications on project.Id equals application.ProjectId
                          join country in ReadContext.Countries on project.CountryId equals country.Id
                          where project.Name.Contains(projectName)
                          select new ProjectDetails(project, scoring, application, country)).ToArrayAsync();
        }
    }
}