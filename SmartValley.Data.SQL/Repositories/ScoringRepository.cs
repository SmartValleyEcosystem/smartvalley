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
                var areaScoring = new AreaScoring {ScoringId = scoringId, AreaId = area, IsCompleted = true};
                EditContext.AreaScorings.Attach(areaScoring).Property(s => s.IsCompleted).IsModified = true;
            }

            return EditContext.SaveAsync();
        }

        public Task AddAreasAsync(IReadOnlyCollection<AreaScoring> areaScorings)
        {
            EditContext.AreaScorings.AddRange(areaScorings);
            return EditContext.SaveAsync();
        }

        public async Task<IReadOnlyCollection<ScoringAreaStatistics>> GetIncompletedScoringAreaStatisticsAsync(DateTimeOffset tillDate)
        {
            var requiredCounts = await (from areaScoring in ReadContext.AreaScorings
                                        join scoring in ReadContext.Scorings on areaScoring.ScoringId equals scoring.Id
                                        where !areaScoring.IsCompleted
                                        select new
                                               {
                                                   OffersEndDate = scoring.OffersDueDate,
                                                   ScoringEndDate = scoring.EstimatesDueDate,
                                                   areaScoring.ScoringId,
                                                   AreaType = areaScoring.AreaId,
                                                   Count = areaScoring.ExpertsCount
                                               }).ToArrayAsync();

            var finishedCounts = await (from areaScoring in ReadContext.AreaScorings
                                        join scoringOffer in ReadContext.ScoringOffers
                                            on new {areaScoring.ScoringId, areaScoring.AreaId}
                                            equals new {scoringOffer.ScoringId, scoringOffer.AreaId}
                                        where scoringOffer.Status == ScoringOfferStatus.Finished
                                        where !areaScoring.IsCompleted
                                        group scoringOffer by new {scoringOffer.ScoringId, scoringOffer.AreaId}
                                        into scoringAreaGroup
                                        select new
                                               {
                                                   scoringAreaGroup.Key.ScoringId,
                                                   AreaType = scoringAreaGroup.Key.AreaId,
                                                   Count = scoringAreaGroup.Count()
                                               }).ToArrayAsync();

            var acceptedCounts = await (from areaScoring in ReadContext.AreaScorings
                                        join scoringOffer in ReadContext.ScoringOffers
                                            on new {areaScoring.ScoringId, areaScoring.AreaId}
                                            equals new {scoringOffer.ScoringId, scoringOffer.AreaId}
                                        where scoringOffer.Status == ScoringOfferStatus.Accepted
                                        where !areaScoring.IsCompleted
                                        group scoringOffer by new {scoringOffer.ScoringId, scoringOffer.AreaId}
                                        into scoringAreaGroup
                                        select new
                                               {
                                                   scoringAreaGroup.Key.ScoringId,
                                                   AreaType = scoringAreaGroup.Key.AreaId,
                                                   Count = scoringAreaGroup.Count()
                                               }).ToArrayAsync();

            var pendingCounts = await (from areaScoring in ReadContext.AreaScorings
                                       join scoringOffer in ReadContext.ScoringOffers
                                           on new {areaScoring.ScoringId, areaScoring.AreaId}
                                           equals new {scoringOffer.ScoringId, scoringOffer.AreaId}
                                       where scoringOffer.Status == ScoringOfferStatus.Pending
                                       where !areaScoring.IsCompleted
                                       where scoringOffer.ExpirationTimestamp >= tillDate
                                       group scoringOffer by new {scoringOffer.ScoringId, scoringOffer.AreaId}
                                       into scoringAreaGroup
                                       select new
                                              {
                                                  scoringAreaGroup.Key.ScoringId,
                                                  AreaType = scoringAreaGroup.Key.AreaId,
                                                  Count = scoringAreaGroup.Count()
                                              }).ToArrayAsync();

            return requiredCounts.Select(i =>
                                             new ScoringAreaStatistics(
                                                 i.AreaType,
                                                 i.ScoringId,
                                                 i.Count,
                                                 acceptedCounts.FirstOrDefault(j => j.ScoringId == i.ScoringId && j.AreaType == i.AreaType)?.Count ?? 0,
                                                 pendingCounts.FirstOrDefault(j => j.ScoringId == i.ScoringId && j.AreaType == i.AreaType)?.Count ?? 0,
                                                 finishedCounts.FirstOrDefault(j => j.ScoringId == i.ScoringId && j.AreaType == i.AreaType)?.Count ?? 0,
                                                 i.ScoringEndDate,
                                                 i.OffersEndDate
                                             )).ToArray();
        }

        public async Task<bool> HasEnoughExpertsAsync(long scoringId)
        {
            var scoringAreaCounts = await (from areaScoring in ReadContext.AreaScorings
                                           join scoringOffer in ReadContext.ScoringOffers
                                               on new {areaScoring.ScoringId, areaScoring.AreaId}
                                               equals new {scoringOffer.ScoringId, scoringOffer.AreaId}
                                           where scoringOffer.Status == ScoringOfferStatus.Accepted
                                           where !areaScoring.IsCompleted
                                           where areaScoring.ScoringId == scoringId
                                           group scoringOffer by new {scoringOffer.ScoringId, scoringOffer.AreaId, areaScoring.ExpertsCount}
                                           into scoringAreaGroup
                                           select new
                                                  {
                                                      Count = scoringAreaGroup.Count(),
                                                      RequiredCount = scoringAreaGroup.Key.ExpertsCount
                                                  }).ToArrayAsync();
            return scoringAreaCounts.All(i => i.RequiredCount == i.Count);
        }

        public async Task<IReadOnlyCollection<ScoringProjectDetails>> GetScoringProjectsDetailsByScoringIdsAsync(IReadOnlyCollection<long> scoringIds)
        {
            return await (from project in ReadContext.Projects
                          join scoring in ReadContext.Scorings on project.Id equals scoring.ProjectId
                          where scoringIds.Any(i => i.Equals(scoring.Id))
                          select new ScoringProjectDetails(project.Id, scoring.Id, scoring.ContractAddress, project.Name, scoring.CreationDate, scoring.OffersDueDate)).ToArrayAsync();
        }
    }
}