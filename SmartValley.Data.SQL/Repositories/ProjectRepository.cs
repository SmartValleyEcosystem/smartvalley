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

        public async Task<IReadOnlyCollection<ProjectDetails>> QueryAsync(ProjectsQuery projectsQuery)
            => await GetQueryable(projectsQuery).ToArrayAsync();

        public Task<int> GetQueryTotalCountAsync(ProjectsQuery projectsQuery)
            => GetQueryable(projectsQuery, false, false).CountAsync();

        public Task<ProjectDetails> GetByAuthorIdAsync(long authorId)
        {
            return (from project in ReadContext.Projects
                    from scoring in ReadContext.Scorings.Where(s => s.ProjectId == project.Id).DefaultIfEmpty()
                    join country in ReadContext.Countries on project.CountryId equals country.Id
                    where project.AuthorId == authorId
                    select new ProjectDetails(project, scoring, country)).FirstOrDefaultAsync();
        }

        public Task<Project> GetByExternalIdAsync(Guid externalId)
            => ReadContext.Projects.FirstAsync(project => project.ExternalId == externalId);

        public async Task<IReadOnlyCollection<ProjectDetails>> GetByExternalIdsAsync(IReadOnlyCollection<Guid> externalIds)
        {
            return await (from project in ReadContext.Projects
                          join scoring in ReadContext.Scorings on project.Id equals scoring.ProjectId into s
                          from scoring in s.DefaultIfEmpty()
                          join country in ReadContext.Countries on project.CountryId equals country.Id
                          where externalIds.Contains(project.ExternalId)
                          select new ProjectDetails(project, scoring, country)).ToArrayAsync();
        }

        public async Task<IReadOnlyCollection<ProjectDetails>> GetAllByNameAsync(string projectName)
        {
            return await (from project in ReadContext.Projects
                          join scoring in ReadContext.Scorings on project.Id equals scoring.ProjectId into s
                          from scoring in s.DefaultIfEmpty()
                          join country in ReadContext.Countries on project.CountryId equals country.Id
                          where project.Name.Contains(projectName)
                          select new ProjectDetails(project, scoring, country)).ToArrayAsync();
        }

        private IQueryable<ProjectDetails> GetQueryable(ProjectsQuery query, bool enablePaging = true, bool enableSorting = true)
        {
            var queryable = from project in ReadContext.Projects
                            join scoring in ReadContext.Scorings on project.Id equals scoring.ProjectId into s
                            from scoring in s.DefaultIfEmpty()
                            join country in ReadContext.Countries on project.CountryId equals country.Id
                            where !query.OnlyScored || scoring.Score.HasValue
                            where !query.Stage.HasValue || project.Stage == query.Stage.Value
                            where string.IsNullOrEmpty(query.SearchString) || project.Name.ToUpper().Contains(query.SearchString.ToUpper())
                            where string.IsNullOrEmpty(query.CountryCode) || country.Code == query.CountryCode.ToUpper()
                            where !query.Category.HasValue || project.Category == query.Category.Value
                            where !query.MinimumScore.HasValue || !scoring.Score.HasValue || scoring.Score >= query.MinimumScore.Value
                            where !query.MaximumScore.HasValue || !scoring.Score.HasValue || scoring.Score <= query.MaximumScore.Value
                            select new {project, scoring, country};

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
                    case ProjectsOrderBy.CreationDate:
                        queryable = query.Direction == SortDirection.Ascending
                                        ? queryable.OrderBy(o => o.project.Id)
                                        : queryable.OrderByDescending(o => o.project.Id);
                        break;
                }
            }

            if (enablePaging)
            {
                queryable = queryable
                            .Skip(query.Offset)
                            .Take(query.Count);
            }

            return queryable.Select(o => new ProjectDetails(o.project, o.scoring, o.country));
        }
    }
}