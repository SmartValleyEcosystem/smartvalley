using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain;
using SmartValley.Domain.Entities;

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

        public Task<IReadOnlyCollection<ScoringOfferDetails>> GetAllPendingByExpertAsync(string expertAddress)
            => GetAllForExpertByStatusAsync(expertAddress, ScoringOfferStatus.Pending);

        public Task<IReadOnlyCollection<ScoringOfferDetails>> GetAllAcceptedByExpertAsync(string expertAddress)
            => GetAllForExpertByStatusAsync(expertAddress, ScoringOfferStatus.Accepted);

        private async Task<IReadOnlyCollection<ScoringOfferDetails>> GetAllForExpertByStatusAsync(string expertAddress, ScoringOfferStatus status)
        {
            return await (from scoringOffer in _readContext.ScoringOffers
                          join scoring in _readContext.Scorings on scoringOffer.ScoringId equals scoring.Id
                          join project in _readContext.Projects on scoring.ProjectId equals project.Id
                          join user in _readContext.Users on scoringOffer.ExpertId equals user.Id
                          where scoringOffer.Status == status && user.Address.Equals(expertAddress, StringComparison.OrdinalIgnoreCase)
                          select new ScoringOfferDetails
                                 {
                                     ScoringOfferStatus = scoringOffer.Status,
                                     ScoringOfferTimestamp = scoringOffer.ExpirationTimestamp,
                                     AreaType = scoringOffer.AreaId,
                                     Name = project.Name,
                                     ProjectArea = project.ProjectArea,
                                     Description = project.Description,
                                     ProjectExternalId = project.ExternalId,
                                     Country = project.Country,
                                     ScoringContractAddress = scoring.ContractAddress,
                                     ExpertId = user.Id,
                                     ScoringId = scoring.Id
                                 })
                       .ToArrayAsync();
        }
    }
}