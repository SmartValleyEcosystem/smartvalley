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
    public class EstimateRepository : EntityCrudRepository<Estimate>, IEstimateRepository
    {
        public EstimateRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
            : base(readContext, editContext)
        {
        }

        public async Task<IReadOnlyCollection<Estimate>> GetAsync(long projectId)
            => await ReadContext.Estimates.Where(e => e.ProjectId == projectId).ToArrayAsync();

        public async Task<IReadOnlyCollection<Estimate>> GetAsync(long projectId, ExpertiseArea expertiseArea)
        {
            return await (from estimate in ReadContext.Estimates
                          join question in ReadContext.Questions on estimate.QuestionId equals question.Id
                          where estimate.ProjectId == projectId && question.ExpertiseArea == expertiseArea
                          select estimate)
                       .ToArrayAsync();
        }

        public async Task<IReadOnlyCollection<long>> GetProjectsEstimatedByExpertAsync(string expertAddress, ExpertiseArea expertiseArea)
        {
            return await (from estimate in ReadContext.Estimates
                          join question in ReadContext.Questions on estimate.QuestionId equals question.Id
                          where estimate.ExpertAddress.Equals(expertAddress, StringComparison.OrdinalIgnoreCase) && question.ExpertiseArea == expertiseArea
                          select estimate.ProjectId)
                       .Distinct()
                       .ToArrayAsync();
        }
    }
}