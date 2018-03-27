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

        private IQueryable<ProjectDetails> GetScoredQueryable(SearchProjectsQuery query, bool enablePaging = true, bool enableSorting = true)
        {
            var queryable = ReadContext
                            .Projects
                            .Join(ReadContext.Scorings, p => p.Id, s => s.ProjectId, (project, scoring) => new {project, scoring})
                            .Join(ReadContext.Applications, p => p.project.Id, a => a.ProjectId, (current, application) => new {current.project, current.scoring, application})
                            .Join(ReadContext.Countries, p => p.project.CountryId, c => c.Id, (current, country) => new {current.project, current.scoring, current.application, country})
                            .Where(o => o.scoring.Score.HasValue)
                            .Where(o => !query.StageType.HasValue || o.project.StageId == query.StageType.Value)
                            .Where(o => string.IsNullOrEmpty(query.SearchString) || o.project.Name.ToUpper().Contains(query.SearchString.ToUpper()))
                            .Where(o => string.IsNullOrEmpty(query.CountryCode) || o.country.Code == query.CountryCode.ToUpper())
                            .Where(o => !query.CategoryType.HasValue || o.project.CategoryId == query.CategoryType.Value)
                            .Where(o => !query.MinimumScore.HasValue || o.scoring.Score >= query.MinimumScore.Value)
                            .Where(o => !query.MaximumScore.HasValue || o.scoring.Score <= query.MaximumScore.Value);

            if (enableSorting && query.OrderBy.HasValue)
            {
                switch (query.OrderBy)
                {
                    case ProjectsOrderBy.Name:
                        queryable = query.Direction == SortDirection.Ascending
                                        ? queryable.OrderBy(o => o.project.Name)
                                        : queryable.OrderByDescending(i => i.project.Name);
                        break;
                    case ProjectsOrderBy.ScoringRating:
                        queryable = query.Direction == SortDirection.Ascending
                                        ? queryable.OrderBy(o => o.scoring.Score)
                                        : queryable.OrderByDescending(i => i.scoring.Score);
                        break;
                    case ProjectsOrderBy.ScoringEndDate:
                        queryable = query.Direction == SortDirection.Ascending
                                        ? queryable.OrderBy(o => o.scoring.ScoringEndDate)
                                        : queryable.OrderByDescending(o => o.scoring.ScoringEndDate);
                        break;
                }
            }

            if (enablePaging)
            {
                queryable = queryable
                            .Skip(query.Offset)
                            .Take(query.Count);
            }

            return queryable.Select(o => new ProjectDetails(o.project, o.scoring, o.application, o.country));
        }

        public async Task<IReadOnlyCollection<ProjectDetails>> GetScoredAsync(SearchProjectsQuery projectsQuery)
            => await GetScoredQueryable(projectsQuery).ToArrayAsync();

        public Task<int> GetScoredTotalCountAsync(SearchProjectsQuery projectsQuery)
            => GetScoredQueryable(projectsQuery, false, false).CountAsync();

        public async Task<IReadOnlyCollection<ProjectDetails>> GetByAuthorIdAsync(long authorId)
        {
            return await (from project in ReadContext.Projects
                          from scoring in ReadContext.Scorings.Where(s => s.ProjectId == project.Id).DefaultIfEmpty()
                          from application in ReadContext.Applications.Where(a => a.ProjectId == project.Id).DefaultIfEmpty()
                          join country in ReadContext.Countries on project.CountryId equals country.Id
                          where project.AuthorId == authorId
                          select new ProjectDetails(project, scoring, application, country)).ToArrayAsync();
        }

        public Task<int> UpdateAsync(Project project)
        {
            EditContext.Projects.Attach(project).State = EntityState.Modified;
            EditContext.Entity(project.SocialNetworks).State = EntityState.Modified;
            return EditContext.SaveAsync();
        }

        public async Task<IReadOnlyCollection<ProjectDetails>> GetForScoringAsync(AreaType areaType, long expertId)
        {
            var scoredProjects = (from scoringCriterion in ReadContext.ScoringCriteria
                                  join comment in ReadContext.EstimateComments on scoringCriterion.Id equals comment.ScoringCriterionId
                                  join scoring in ReadContext.Scorings on comment.ScoringId equals scoring.Id
                                  join areaScoring in ReadContext.AreaScorings on scoring.Id equals areaScoring.ScoringId
                                  where comment.ExpertId == expertId && scoringCriterion.AreaType == areaType ||
                                        areaScoring.AreaId == areaType && areaScoring.Score.HasValue
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