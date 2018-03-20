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

        private IQueryable<ProjectDetails> GetScoredQueryable(SearchProjectsQuery projectsQuery)
        {
            var query = from project in ReadContext.Projects
                        join scoring in ReadContext.Scorings on project.Id equals scoring.ProjectId
                        join application in ReadContext.Applications on project.Id equals application.ProjectId
                        join country in ReadContext.Countries on project.CountryId equals country.Id
                        where scoring.Score.HasValue
                        where !projectsQuery.StageType.HasValue || project.StageId == projectsQuery.StageType.Value
                        where string.IsNullOrEmpty(projectsQuery.SearchString) || project.Name.ToUpper().Contains(projectsQuery.SearchString.ToUpper())
                        where string.IsNullOrEmpty(projectsQuery.CountryCode) || country.Code == projectsQuery.CountryCode.ToUpper()
                        where !projectsQuery.CategoryType.HasValue || project.CategoryId == projectsQuery.CategoryType.Value
                        where !projectsQuery.MinimumScore.HasValue || scoring.Score >= projectsQuery.MinimumScore.Value
                        where !projectsQuery.MaximumScore.HasValue || scoring.Score <= projectsQuery.MaximumScore.Value
                        select new ProjectDetails(project, scoring, application, country);
            return AddSorting(query, projectsQuery.OrderBy, projectsQuery.Direction);
        }

        public async Task<IReadOnlyCollection<ProjectDetails>> GetScoredAsync(SearchProjectsQuery projectsQuery)
        {
            return await GetScoredQueryable(projectsQuery)
                       .Skip(projectsQuery.Offset)
                       .Take(projectsQuery.Count)
                       .ToArrayAsync();
        }

        public Task<int> GetScoredTotalCountAsync(SearchProjectsQuery projectsQuery)
        {
            return GetScoredQueryable(projectsQuery).CountAsync();
        }

        private IQueryable<ProjectDetails> AddSorting(IQueryable<ProjectDetails> details, ProjectsOrderBy orderBy, SortDirection direction)
        {
            switch (orderBy)
            {
                case ProjectsOrderBy.Name:
                    return @direction == SortDirection.Ascending 
                        ? details.OrderBy(i => i.Project.Name) 
                        : details.OrderByDescending(i => i.Project.Name);
                case ProjectsOrderBy.ScoringRating:
                    return @direction == SortDirection.Ascending
                        ? details.OrderBy(i => i.Scoring.Score) 
                        : details.OrderByDescending(i => i.Scoring.Score);
                default:
                    return @direction == SortDirection.Ascending
                        ? details.OrderBy(i => i.Scoring.ScoringEndDate) 
                        : details.OrderByDescending(i => i.Scoring.ScoringEndDate);
            }
        }

        public async Task<IReadOnlyCollection<ProjectDetails>> GetByAuthorIdAsync(long authorId)
        {
            return await (from project in ReadContext.Projects
                          from scoring in ReadContext.Scorings.Where(s => s.ProjectId == project.Id).DefaultIfEmpty()
                          from application in ReadContext.Applications.Where(a => a.ProjectId == project.Id).DefaultIfEmpty()
                          join country in ReadContext.Countries on project.CountryId equals country.Id
                          where project.AuthorId == authorId
                          select new ProjectDetails(project, scoring, application, country)).ToArrayAsync();
        }

        public async Task<IReadOnlyCollection<ProjectDetails>> GetForScoringAsync(AreaType areaType, long expertId)
        {
            var scoredProjects = (from question in ReadContext.Questions
                                  join comment in ReadContext.EstimateComments on question.Id equals comment.QuestionId
                                  join scoring in ReadContext.Scorings on comment.ScoringId equals scoring.Id
                                  join areaScoring in ReadContext.AreaScorings on scoring.Id equals areaScoring.ScoringId
                                  where comment.ExpertId == expertId && question.AreaType == areaType ||
                                        areaScoring.AreaId == areaType && areaScoring.IsCompleted
                                  select comment.ScoringId).Distinct();

            return await (from project in ReadContext.Projects
                          join scoring in ReadContext.Scorings on project.Id equals scoring.ProjectId
                          join application in ReadContext.Applications on project.Id equals application.ProjectId
                          join country in ReadContext.Countries on project.CountryId equals country.Id
                          where !scoredProjects.Contains(scoring.Id)
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