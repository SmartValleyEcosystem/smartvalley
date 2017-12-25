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
            var scoredProjects = (from question in ReadContext.Questions
                                  join estimateComment in ReadContext.EstimateComments on question.Id equals estimateComment.QuestionId
                                  where estimateComment.ExpertAddress.Equals(expertAddress, StringComparison.OrdinalIgnoreCase)
                                        && question.ExpertiseArea == expertiseArea
                                  select estimateComment.ProjectId)
                .Distinct();

            return await GetByExpertiseArea(expertiseArea)
                       .Where(p => !scoredProjects.Contains(p.Id))
                       .ToArrayAsync();
        }

        private IQueryable<Project> GetByExpertiseArea(ExpertiseArea expertiseArea)
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