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

        public async Task<IReadOnlyCollection<ScoringAreaStatistics>> GetIncompletedScoringAreaStatisticsAsync(DateTimeOffset now)
        {
            var incompleteAreaScorings = _readContext.AreaScorings.Where(a => !a.Score.HasValue);

            var requiredCounts = await (from areaScoring in incompleteAreaScorings
                                        join scoring in _readContext.Scorings on areaScoring.ScoringId equals scoring.Id
                                        select new
                                               {
                                                   scoring.AcceptingDeadline,
                                                   scoring.ScoringDeadline,
                                                   areaScoring.ScoringId,
                                                   AreaType = areaScoring.AreaId,
                                                   Count = areaScoring.ExpertsCount
                                               }).ToArrayAsync();

            var acceptedCounts = await (from areaScoring in incompleteAreaScorings
                                        join scoringOffer in _readContext.ScoringOffers
                                            on new {areaScoring.ScoringId, areaScoring.AreaId}
                                            equals new {scoringOffer.ScoringId, scoringOffer.AreaId}
                                        where scoringOffer.Status == ScoringOfferStatus.Accepted
                                        group scoringOffer by new {scoringOffer.ScoringId, scoringOffer.AreaId}
                                        into scoringAreaGroup
                                        select new
                                               {
                                                   scoringAreaGroup.Key.ScoringId,
                                                   AreaType = scoringAreaGroup.Key.AreaId,
                                                   Count = scoringAreaGroup.Count()
                                               }).ToArrayAsync();

            var pendingCounts = await (from areaScoring in incompleteAreaScorings
                                       join scoringOffer in _readContext.ScoringOffers
                                           on new {areaScoring.ScoringId, areaScoring.AreaId}
                                           equals new {scoringOffer.ScoringId, scoringOffer.AreaId}
                                       where scoringOffer.Status == ScoringOfferStatus.Pending
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
                                             i.ScoringDeadline,
                                             i.AcceptingDeadline
                                         )).ToArray();
        }

        public Task SaveChangesAsync()
            => _editContext.SaveAsync();

        private IQueryable<Scoring> Entities()
            => _editContext.Scorings
                           .Include(x => x.ExpertScorings).ThenInclude(x => x.Estimates)
                           .Include(x => x.ScoringOffers)
                           .Include(x => x.AreaScorings);
    }
}