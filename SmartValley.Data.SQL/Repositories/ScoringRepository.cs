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
    public class ScoringRepository : IScoringRepository
    {
        private readonly IReadOnlyDataContext _readContext;
        private readonly IEditableDataContext _editContext;

        public ScoringRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
        {
            _readContext = readContext;
            _editContext = editContext;
        }

        public void Add(Scoring scoring)
        {
            _editContext.Scorings.Add(scoring);
        }

        public Task<Scoring> GetByProjectIdAsync(long projectId)
            => Entities().FirstOrDefaultAsync(scoring => scoring.ProjectId == projectId);

        public Task<Scoring> GetByIdAsync(long scoringId)
            => Entities().FirstOrDefaultAsync(scoring => scoring.Id == scoringId);

        public async Task<IReadOnlyCollection<ScoringAreaStatistics>> GetIncompletedScoringAreaStatisticsAsync(DateTimeOffset tillDate)
        {
            var requiredCounts = await (from areaScoring in _readContext.AreaScorings
                                        join scoring in _readContext.Scorings on areaScoring.ScoringId equals scoring.Id
                                        where !areaScoring.Score.HasValue
                                        select new
                                               {
                                                   OffersEndDate = scoring.OffersDueDate,
                                                   ScoringEndDate = scoring.EstimatesDueDate,
                                                   areaScoring.ScoringId,
                                                   AreaType = areaScoring.AreaId,
                                                   Count = areaScoring.ExpertsCount
                                               }).ToArrayAsync();

            var finishedCounts = await (from areaScoring in _readContext.AreaScorings
                                        join scoringOffer in _readContext.ScoringOffers
                                            on new {areaScoring.ScoringId, areaScoring.AreaId}
                                            equals new {scoringOffer.ScoringId, scoringOffer.AreaId}
                                        where scoringOffer.Status == ScoringOfferStatus.Finished
                                        where !areaScoring.Score.HasValue
                                        group scoringOffer by new {scoringOffer.ScoringId, scoringOffer.AreaId}
                                        into scoringAreaGroup
                                        select new
                                               {
                                                   scoringAreaGroup.Key.ScoringId,
                                                   AreaType = scoringAreaGroup.Key.AreaId,
                                                   Count = scoringAreaGroup.Count()
                                               }).ToArrayAsync();

            var acceptedCounts = await (from areaScoring in _readContext.AreaScorings
                                        join scoringOffer in _readContext.ScoringOffers
                                            on new {areaScoring.ScoringId, areaScoring.AreaId}
                                            equals new {scoringOffer.ScoringId, scoringOffer.AreaId}
                                        where scoringOffer.Status == ScoringOfferStatus.Accepted
                                        where !areaScoring.Score.HasValue
                                        group scoringOffer by new {scoringOffer.ScoringId, scoringOffer.AreaId}
                                        into scoringAreaGroup
                                        select new
                                               {
                                                   scoringAreaGroup.Key.ScoringId,
                                                   AreaType = scoringAreaGroup.Key.AreaId,
                                                   Count = scoringAreaGroup.Count()
                                               }).ToArrayAsync();

            var pendingCounts = await (from areaScoring in _readContext.AreaScorings
                                       join scoringOffer in _readContext.ScoringOffers
                                           on new {areaScoring.ScoringId, areaScoring.AreaId}
                                           equals new {scoringOffer.ScoringId, scoringOffer.AreaId}
                                       where scoringOffer.Status == ScoringOfferStatus.Pending
                                       where !areaScoring.Score.HasValue
                                       where scoringOffer.ExpirationTimestamp >= tillDate
                                       group scoringOffer by new {scoringOffer.ScoringId, scoringOffer.AreaId}
                                       into scoringAreaGroup
                                       select new
                                              {
                                                  scoringAreaGroup.Key.ScoringId,
                                                  AreaType = scoringAreaGroup.Key.AreaId,
                                                  Count = scoringAreaGroup.Count()
                                              }).ToArrayAsync();

            return requiredCounts.Select(i => new ScoringAreaStatistics(
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

        public async Task<IReadOnlyCollection<ScoringProjectDetails>> GetScoringProjectsDetailsByScoringIdsAsync(IReadOnlyCollection<long> scoringIds)
        {
            return await (from project in _readContext.Projects
                          join scoring in _readContext.Scorings on project.Id equals scoring.ProjectId
                          where scoringIds.Any(i => i.Equals(scoring.Id))
                          select new ScoringProjectDetails(
                              project.Id,
                              project.ExternalId,
                              scoring.Id,
                              scoring.ContractAddress,
                              project.Name,
                              scoring.CreationDate,
                              scoring.OffersDueDate))
                       .ToArrayAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _editContext.SaveAsync();
        }

        private IQueryable<Scoring> Entities()
            => _editContext.Scorings
                          .Include(x => x.ExpertScorings).ThenInclude(x => x.Estimates)
                          .Include(x => x.ScoringOffers)
                          .Include(x => x.AreaScorings);
    }
}