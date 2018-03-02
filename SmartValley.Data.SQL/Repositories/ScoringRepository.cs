using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Data.SQL.Migrations;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ScoringRepository : EntityCrudRepository<Scoring>, IScoringRepository
    {
        public ScoringRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
            : base(readContext, editContext)
        {
        }

        public Task<Scoring> GetByProjectIdAsync(long projectId)
            => ReadContext.Scorings.FirstOrDefaultAsync(scoring => scoring.ProjectId == projectId);

        public async Task<bool> IsCompletedInAreaAsync(long scoringId, AreaType areaType)
        {
            var areaScoring = await ReadContext.AreaScorings.FirstOrDefaultAsync(s => s.ScoringId == scoringId && s.AreaId == areaType);
            return areaScoring?.IsCompleted == true;
        }

        public Task SetAreasCompletedAsync(long scoringId, IReadOnlyCollection<AreaType> areas)
        {
            foreach (var area in areas)
            {
                var areaScoring = new AreaScoring { ScoringId = scoringId, AreaId = area, IsCompleted = true };
                EditContext.AreaScorings.Attach(areaScoring).Property(s => s.IsCompleted).IsModified = true;
            }

            return EditContext.SaveAsync();
        }

        public Task AddAreasAsync(IReadOnlyCollection<AreaScoring> areaScorings)
        {
            EditContext.AreaScorings.AddRange(areaScorings);
            return EditContext.SaveAsync();
        }

        public async Task<IReadOnlyCollection<ScoringAreaStatistic>> GetIncompletedScoringAreaStatisticsAsync(DateTimeOffset tillDate)
        {
            var requiredCounts = await (from areaScorings in ReadContext.AreaScorings
                                        where !areaScorings.IsCompleted
                                        select new
                                        {
                                            areaScorings.ScoringId,
                                            AreaType = areaScorings.AreaId,
                                            Count = areaScorings.ExpertsCount
                                        }).ToArrayAsync();

            var acceptedCounts = await (from areaScorings in ReadContext.AreaScorings
                                        join scoringOffer in ReadContext.ScoringOffers
                                        on new { areaScorings.ScoringId, areaScorings.AreaId }
                                        equals new { scoringOffer.ScoringId, scoringOffer.AreaId }
                                        where scoringOffer.Status == ScoringOfferStatus.Accepted
                                        where !areaScorings.IsCompleted
                                        group scoringOffer by new { scoringOffer.ScoringId, scoringOffer.AreaId } into grouped
                                        select new
                                        {
                                            grouped.Key.ScoringId,
                                            AreaType = grouped.Key.AreaId,
                                            Count = grouped.Count()
                                        }).ToArrayAsync();

            var pendingCounts = await (from areaScorings in ReadContext.AreaScorings
                                       join scoringOffer in ReadContext.ScoringOffers
                                       on new { areaScorings.ScoringId, areaScorings.AreaId }
                                       equals new { scoringOffer.ScoringId, scoringOffer.AreaId }
                                       where scoringOffer.Status == ScoringOfferStatus.Pending
                                       where !areaScorings.IsCompleted
                                       where scoringOffer.Timestamp <= tillDate
                                       group scoringOffer by new { scoringOffer.ScoringId, scoringOffer.AreaId } into grouped
                                       select new
                                       {
                                           grouped.Key.ScoringId,
                                           AreaType = grouped.Key.AreaId,
                                           Count = grouped.Count()
                                       }).ToArrayAsync();


            return requiredCounts.Select(i =>
                                             new ScoringAreaStatistic
                                             {
                                                 RequiredCount = i.Count,
                                                 AcceptedCount = acceptedCounts.FirstOrDefault(j => j.ScoringId == i.ScoringId && j.AreaType == i.AreaType)?.Count ?? 0,
                                                 PendingCount = pendingCounts.FirstOrDefault(j => j.ScoringId == i.ScoringId && j.AreaType == i.AreaType)?.Count ?? 0,
                                                 ScoringId = i.ScoringId,
                                                 AreaId = i.AreaType
                                             }).ToArray();
        }

        public async Task<IReadOnlyCollection<ScoringProjectDetails>> GetScoringProjectsDetailsByScoringIdsAsync(IEnumerable<long> ids, uint offerExpirationPeriod)
        {
            return await (from projects in ReadContext.Projects
                          join scorings in ReadContext.Scorings on projects.Id equals scorings.ProjectId
                          where ids.Any(i => i.Equals(scorings.Id))
                          select new ScoringProjectDetails
                          {
                              ProjectId = projects.Id,
                              ScoringId = scorings.Id,
                              Address = scorings.ContractAddress,
                              Name = projects.Name
                          })
                                         .ToArrayAsync();
        }
    }
}