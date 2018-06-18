using System;
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
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ScoringOffersRepository : IScoringOffersRepository
    {
        private readonly IReadOnlyDataContext _readContext;

        public ScoringOffersRepository(IReadOnlyDataContext readContext)
        {
            _readContext = readContext;
        }

        public Task<PagingCollection<ScoringOfferDetails>> GetAsync(OffersQuery query, DateTimeOffset now)
        {
            var queryable = from scoringOffer in _readContext.ScoringOffers
                            join scoring in _readContext.Scorings on scoringOffer.ScoringId equals scoring.Id
                            join project in _readContext.Projects on scoring.ProjectId equals project.Id
                            join user in _readContext.Users on scoringOffer.ExpertId equals user.Id
                            join country in _readContext.Countries on project.CountryId equals country.Id
                            where !query.ExpertId.HasValue || user.Id == query.ExpertId.Value
                            where !query.ScoringId.HasValue || scoring.Id == query.ScoringId.Value
                            where !query.ProjectId.HasValue || project.Id == query.ProjectId.Value
                            where !query.Status.HasValue || (query.Status == ScoringOfferStatus.Expired && (scoringOffer.Status == ScoringOfferStatus.Expired
                                                                                                            || scoring.AcceptingDeadline < now && scoringOffer.Status == ScoringOfferStatus.Pending
                                                                                                            || scoring.ScoringDeadline < now && scoringOffer.Status == ScoringOfferStatus.Accepted)
                                                             || (query.Status == scoringOffer.Status
                                                                 && !(scoring.AcceptingDeadline < now && scoringOffer.Status == ScoringOfferStatus.Pending)
                                                                 && !(scoring.ScoringDeadline < now && scoringOffer.Status == ScoringOfferStatus.Accepted)))
                            select new ScoringOfferDetails(scoringOffer.Status,
                                                           scoring.AcceptingDeadline,
                                                           scoring.ScoringDeadline,
                                                           scoring.ContractAddress,
                                                           scoring.Id,
                                                           user.Id,
                                                           project.Name,
                                                           country.Code,
                                                           project.Category,
                                                           project.Description,
                                                           scoringOffer.AreaId,
                                                           project.ExternalId,
                                                           project.Id,
                                                           project.IsPrivate,
                                                           scoring.Score);

            if (query.OrderBy.HasValue)
            {
                switch (query.OrderBy.Value)
                {
                    case ScoringOffersOrderBy.Name:
                        queryable = query.SortDirection == SortDirection.Ascending
                                        ? queryable.OrderBy(o => o.Name)
                                        : queryable.OrderByDescending(o => o.Name);
                        break;
                    case ScoringOffersOrderBy.Status:
                        queryable = query.SortDirection == SortDirection.Ascending
                                        ? queryable.OrderBy(o => o.Status)
                                        : queryable.OrderByDescending(o => o.Status);
                        break;
                    case ScoringOffersOrderBy.Deadline:
                        queryable = query.SortDirection == SortDirection.Ascending
                                        ? queryable.OrderBy(o => o.AcceptingDeadline)
                                        : queryable.OrderByDescending(o => o.AcceptingDeadline);
                        break;
                }
            }

            return queryable.GetPageAsync(query.Offset, query.Count);
        }
    }
}