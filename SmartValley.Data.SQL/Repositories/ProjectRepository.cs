using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class ProjectRepository : EntityCrudRepository<Project>, IProjectRepository
    {
        public ProjectRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
            : base(readContext, editContext)
        {
        }

        public async Task<IReadOnlyCollection<Project>> GetAllScoredAsync()
            => await ReadContext.Projects.Where(project => project.Score.HasValue).ToArrayAsync();

        public async Task<IReadOnlyCollection<Project>> GetAllByAuthorAddressAsync(string address)
        {
            return await ReadContext.Projects
                                    .Where(project => project.AuthorAddress.Equals(address, StringComparison.OrdinalIgnoreCase))
                                    .ToArrayAsync();
        }

        public async Task<IReadOnlyCollection<Project>> GetAllByCategoryAsync(ScoringCategory category)
        {
            var projects = ReadContext.Projects;
            switch (category)
            {
                case ScoringCategory.Unknown:
                    return new List<Project>();
                case ScoringCategory.Hr:
                    return await projects.Where(p => p.HrEstimatesCount < 3).ToArrayAsync();
                case ScoringCategory.Analyst:
                    return await projects.Where(p => p.AnalystEstimatesCount < 3).ToArrayAsync();
                case ScoringCategory.Tech:
                    return await projects.Where(p => p.TechnicalEstimatesCount < 3).ToArrayAsync();
                case ScoringCategory.Lawyer:
                    return await projects.Where(p => p.LawyerEstimatesCount < 3).ToArrayAsync();
                default:
                    return await projects.ToArrayAsync();
            }
        }
    }
}