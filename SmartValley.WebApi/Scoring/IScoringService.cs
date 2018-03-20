using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
using SmartValley.WebApi.Projects;
using SmartValley.WebApi.Scoring.Requests;

namespace SmartValley.WebApi.Scoring
{
    public interface IScoringService
    {
        Task<ScoringOffer> GetOfferAsync(long projectId, AreaType areaType, long expertId);

        Task StartAsync(Guid projectExternalId, IReadOnlyCollection<AreaRequest> areas);

        Task<IReadOnlyCollection<ScoringProjectDetailsWithCounts>> GetScoringProjectsAsync(IReadOnlyCollection<ScoringProjectStatus> statuses);

        Task<IReadOnlyCollection<ScoringOfferDetails>> GetPendingOfferDetailsAsync(long expertId);

        Task<IReadOnlyCollection<ScoringOfferDetails>> GetAcceptedOfferDetailsAsync(long expertId);

        Task<IReadOnlyCollection<ScoringOfferDetails>> GetExpertOffersHistoryAsync(long expertId, DateTimeOffset now);

        Task AcceptOfferAsync(long scoringId, long areaId, long expertId);

        Task RejectOfferAsync(long scoringId, long areaId, long expertId);

        Task UpdateOffersAsync(Guid projectExternalId);
    }
}