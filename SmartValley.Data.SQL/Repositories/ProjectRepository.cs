using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Data.SQL.Extensions;
using SmartValley.Domain;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly IEditableDataContext _editContext;

        public ProjectRepository(IEditableDataContext editContext)
        {
            _editContext = editContext;
        }

        public Task<PagingCollection<Project>> GetAsync(ProjectsQuery query)
        {
            var queryable = from project in Entities()
                            join scoringApplication in _editContext.ScoringApplications on project.Id equals scoringApplication.ProjectId into ps
                            from scoringApplication in ps.DefaultIfEmpty()
                            where !query.OnlyScored || project.Scoring.Score.HasValue
                            where !query.Stage.HasValue || project.Stage == query.Stage.Value
                            where string.IsNullOrEmpty(query.SearchString) || project.Name.ToUpper().Contains(query.SearchString.ToUpper())
                            where string.IsNullOrEmpty(query.CountryCode) || project.Country.Code == query.CountryCode.ToUpper()
                            where !query.Category.HasValue || project.Category == query.Category.Value
                            where !query.MinimumScore.HasValue || (project.Scoring.Score.HasValue && project.Scoring.Score.Value >= query.MinimumScore)
                            where !query.MaximumScore.HasValue || (project.Scoring.Score.HasValue && project.Scoring.Score.Value <= query.MaximumScore)
                            where (!query.IsPrivate.HasValue && project.IsPrivate == false) || (query.IsPrivate.HasValue && project.IsPrivate == query.IsPrivate && scoringApplication.IsSubmitted)
                            where query.ScoringStatuses.Count == 0 || query.ScoringStatuses.Contains(project.Scoring.Status)
                            select project;

            if (query.OrderBy.HasValue)
            {
                switch (query.OrderBy)
                {
                    case ProjectsOrderBy.Name:
                        queryable = query.Direction == SortDirection.Ascending
                                        ? queryable.OrderBy(o => o.Name)
                                        : queryable.OrderByDescending(i => i.Name);
                        break;
                    case ProjectsOrderBy.ScoringRating:
                        queryable = query.Direction == SortDirection.Ascending
                                        ? queryable.OrderBy(o => o.Scoring.Score)
                                        : queryable.OrderByDescending(i => i.Scoring.Score);
                        break;
                    case ProjectsOrderBy.ScoringEndDate:
                        queryable = query.Direction == SortDirection.Ascending
                                        ? queryable.OrderBy(o => o.Scoring.ScoringEndDate)
                                        : queryable.OrderByDescending(o => o.Scoring.ScoringEndDate);
                        break;
                    case ProjectsOrderBy.CreationDate:
                        queryable = query.Direction == SortDirection.Ascending
                                        ? queryable.OrderBy(o => o.Id)
                                        : queryable.OrderByDescending(o => o.Id);
                        break;
                }
            }

            return queryable.GetPageAsync(query.Offset, query.Count);
        }

        public void Add(Project project)
        {
            _editContext.Projects.Add(project);
        }

        public Task<Project> GetByAuthorIdAsync(long authorId)
            => Entities().FirstOrDefaultAsync(p => p.AuthorId == authorId);

        public Task<Project> GetByIdAsync(long projectId)
            => Entities().FirstOrDefaultAsync(p => p.Id == projectId);

        public Task SaveChangesAsync()
            => _editContext.SaveAsync();

        public Task<Project> GetByExternalIdAsync(Guid externalId)
            => Entities().FirstOrDefaultAsync(project => project.ExternalId == externalId);

        public void Remove(Project entity)
        {
            _editContext.Projects.Remove(entity);
        }

        public async Task<IReadOnlyCollection<Project>> GetAllByNameAsync(string projectName)
            => await Entities().Where(p => p.Name.Contains(projectName)).ToArrayAsync();

        private IQueryable<Project> Entities() => _editContext.Projects
                                                              .Include(p => p.Author)
                                                              .Include(p => p.Country)
                                                              .Include(p => p.TeamMembers)
                                                              .Include(p => p.Scoring).ThenInclude(s => s.ExpertScorings);
    }
}