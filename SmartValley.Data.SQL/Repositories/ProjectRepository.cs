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
            return await ReadContext
                       .Projects
                       .Where(project => project.AuthorAddress.Equals(address, StringComparison.OrdinalIgnoreCase))
                       .ToArrayAsync();
        }

        public async Task<IReadOnlyCollection<Project>> GetForScoringAsync(string expertAddress, ExpertiseArea expertiseArea)
        {
            var scoredProjects = ReadContext
                .ScoredProjects
                .Where(p => p.ExpertAddress == expertAddress && p.ExpertiseArea == expertiseArea)
                .Select(p => p.ProjectId);

            return await GetExpertiseArea(expertiseArea)
                       .Where(p => !scoredProjects.Contains(p.Id))
                       .ToArrayAsync();
        }

        private IQueryable<Project> GetExpertiseArea(ExpertiseArea expertiseArea)
        {
            var projects = ReadContext.Projects;
            switch (expertiseArea)
            {
                case ExpertiseArea.Hr:
                    return projects.Where(p => !p.IsScoredByHr);
                case ExpertiseArea.Analyst:
                    return projects.Where(p => !p.IsScoredByAnalyst);
                case ExpertiseArea.Tech:
                    return projects.Where(p => !p.IsScoredByTechnical);
                case ExpertiseArea.Lawyer:
                    return projects.Where(p => !p.IsScoredByLawyer);
                default:
                    return projects;
            }
        }
    }
}