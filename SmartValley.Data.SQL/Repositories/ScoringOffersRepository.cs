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

        public Task AddAsync(IReadOnlyCollection<ScoringOffer> offers)
        {
            _editContext.ScoringOffers.AddRange(offers);
            return _editContext.SaveAsync();
        }

        public Task AcceptAsync(long scoringId, long expertId, AreaType area)
            => SetStatusAsync(scoringId, expertId, area, ScoringOfferStatus.Accepted);

        public Task RejectAsync(long scoringId, long expertId, AreaType area)
            => SetStatusAsync(scoringId, expertId, area, ScoringOfferStatus.Rejected);

        public Task FinishAsync(long scoringId, long expertId, AreaType area)
            => SetStatusAsync(scoringId, expertId, area, ScoringOfferStatus.Finished);

        public async Task<bool> IsAcceptedAsync(long scoringId, long expertId, AreaType area)
        {
            var offer = await _readContext.ScoringOffers.FirstOrDefaultAsync(o => o.ScoringId == scoringId && o.AreaId == area && o.ExpertId == expertId);
            return offer?.Status == ScoringOfferStatus.Accepted;
        }

        public Task<IReadOnlyCollection<ScoringOfferDetails>> GetAllPendingByExpertAsync(long expertId)
            => GetAllForExpertByStatusAsync(expertId, ScoringOfferStatus.Pending);

        public Task<IReadOnlyCollection<ScoringOfferDetails>> GetAllAcceptedByExpertAsync(long expertId)
            => GetAllForExpertByStatusAsync(expertId, ScoringOfferStatus.Accepted);

        public async Task<IReadOnlyCollection<ScoringOfferDetails>> GetExpertOffersHistoryAsync(long expertId, DateTimeOffset now)
        {
            return await (from scoringOffer in _readContext.ScoringOffers
                          join scoring in _readContext.Scorings on scoringOffer.ScoringId equals scoring.Id
                          join project in _readContext.Projects on scoring.ProjectId equals project.Id
                          join user in _readContext.Users on scoringOffer.ExpertId equals user.Id
                          join category in _readContext.Categories on project.CategoryId equals category.Id
                          join country in _readContext.Countries on project.CountryId equals country.Id
                          where scoringOffer.Status != ScoringOfferStatus.Pending || scoringOffer.ExpirationTimestamp < now
                          where user.Id == expertId
                          select new ScoringOfferDetails(
                              scoringOffer.Status,
                              scoringOffer.ExpirationTimestamp,
                              scoring.ContractAddress,
                              scoring.Id,
                              user.Id,
                              project.Name,
                              country.Code,
                              category.Name,
                              project.Description,
                              scoringOffer.AreaId,
                              project.ExternalId))
                       .ToArrayAsync();
        }

        public async Task<IReadOnlyCollection<ScoringOffer>> GetByScoringIdAsync(long scoringId)
            => await _readContext.ScoringOffers.Where(o => o.ScoringId == scoringId).ToArrayAsync();

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

        private async Task<IReadOnlyCollection<ScoringOfferDetails>> GetAllForExpertByStatusAsync(long expertId, ScoringOfferStatus status)
        {
            return await (from scoringOffer in _readContext.ScoringOffers
                          join scoring in _readContext.Scorings on scoringOffer.ScoringId equals scoring.Id
                          join project in _readContext.Projects on scoring.ProjectId equals project.Id
                          join user in _readContext.Users on scoringOffer.ExpertId equals user.Id
                          join category in _readContext.Categories on project.CategoryId equals category.Id
                          join country in _readContext.Countries on project.CountryId equals country.Id
                          where scoringOffer.Status == status
                          where user.Id == expertId
                          select new ScoringOfferDetails(
                              scoringOffer.Status,
                              scoringOffer.ExpirationTimestamp,
                              scoring.ContractAddress,
                              scoring.Id,
                              user.Id,
                              project.Name,
                              country.Code,
                              category.Name,
                              project.Description,
                              scoringOffer.AreaId,
                              project.ExternalId))
                       .ToArrayAsync();
        }
    }
}