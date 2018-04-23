using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ScoringOffersRepository : IScoringOffersRepository
    {
        private readonly IReadOnlyDataContext _readContext;
        private readonly IEditableDataContext _editContext;

        public ScoringOffersRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
        {
            _readContext = readContext;
            _editContext = editContext;
        }

        public Task<ScoringOffer> GetAsync(long projectId, AreaType areaType, long expertId)
        {
            return (from offer in _readContext.ScoringOffers
                    join scoring in _readContext.Scorings on offer.ScoringId equals scoring.Id
                    where scoring.ProjectId == projectId
                    where offer.AreaId == areaType
                    where offer.ExpertId == expertId
                    select offer)
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyCollection<ScoringOffer>> GetByScoringAsync(long scoringId)
            => await _readContext.ScoringOffers.Where(o => o.ScoringId == scoringId).ToArrayAsync();

        public Task AddAsync(IReadOnlyCollection<ScoringOffer> offers)
        {
            _editContext.ScoringOffers.AddRange(offers);
            return _editContext.SaveAsync();
        }

        public Task AcceptAsync(long scoringId, long expertId, AreaType area)
            => SetStatusAsync(scoringId, expertId, area, ScoringOfferStatus.Accepted);

        public Task RejectAsync(long scoringId, long expertId, AreaType area)
            => SetStatusAsync(scoringId, expertId, area, ScoringOfferStatus.Rejected);

        public async Task<IReadOnlyCollection<ScoringOfferDetails>> QueryAsync(OffersQuery query, DateTimeOffset now)
            => await GetQueryable(query, now).ToArrayAsync();

        public async Task<int> GetQueryCountAsync(OffersQuery query, DateTimeOffset now)
            => await GetQueryable(query, now, false, false).CountAsync();

        private IQueryable<ScoringOfferDetails> GetQueryable(OffersQuery query, DateTimeOffset now, bool enableSorting = true, bool enablePaging = true)
        {
            var queryable = from scoringOffer in _readContext.ScoringOffers
                            join scoring in _readContext.Scorings on scoringOffer.ScoringId equals scoring.Id
                            join project in _readContext.Projects on scoring.ProjectId equals project.Id
                            join user in _readContext.Users on scoringOffer.ExpertId equals user.Id
                            join country in _readContext.Countries on project.CountryId equals country.Id
                            where user.Id == query.ExpertId
                            where !query.OnlyTimedOut || scoringOffer.ExpirationTimestamp < now
                            where !query.Status.HasValue || query.Status == scoringOffer.Status
                            select new {scoringOffer, scoring, project, user, country};

            if (enableSorting && query.OrderBy.HasValue)
            {
                switch (query.OrderBy.Value)
                {
                    case ScoringOffersOrderBy.Name:
                        queryable = query.SortDirection == SortDirection.Ascending
                                        ? queryable.OrderBy(o => o.project.Name)
                                        : queryable.OrderByDescending(o => o.project.Name);
                        break;
                    case ScoringOffersOrderBy.Status:
                        queryable = query.SortDirection == SortDirection.Ascending
                                        ? queryable.OrderBy(o => o.scoringOffer.Status)
                                        : queryable.OrderByDescending(o => o.scoringOffer.Status);
                        break;
                    case ScoringOffersOrderBy.Deadline:
                        queryable = query.SortDirection == SortDirection.Ascending
                                        ? queryable.OrderBy(o => o.scoringOffer.ExpirationTimestamp)
                                        : queryable.OrderByDescending(o => o.scoringOffer.ExpirationTimestamp);
                        break;
                }
            }

            if (enablePaging)
            {
                queryable = queryable
                            .Skip(query.Offset)
                            .Take(query.Count);
            }

            return queryable.Select(o => new ScoringOfferDetails(o.scoringOffer.Status,
                                                                 o.scoringOffer.ExpirationTimestamp,
                                                                 o.scoring.EstimatesDueDate,
                                                                 o.scoring.ContractAddress,
                                                                 o.scoring.Id,
                                                                 o.user.Id,
                                                                 o.project.Name,
                                                                 o.country.Code,
                                                                 o.project.Category,
                                                                 o.project.Description,
                                                                 o.scoringOffer.AreaId,
                                                                 o.project.ExternalId,
                                                                 o.project.Id,
                                                                 o.scoring.Score));
        }

        private Task SetStatusAsync(long scoringId, long expertId, AreaType area, ScoringOfferStatus status)
        {
            var scoringOffer = new ScoringOffer
                               {
                                   ScoringId = scoringId,
                                   ExpertId = expertId,
                                   AreaId = area,
                                   Status = status
                               };

            _editContext.ScoringOffers.Attach(scoringOffer).Property(s => s.Status).IsModified = true;
            return _editContext.SaveAsync();
        }
    }
}