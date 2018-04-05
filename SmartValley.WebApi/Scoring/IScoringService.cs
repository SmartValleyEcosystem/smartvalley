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

        Task AcceptOfferAsync(long scoringId, long areaId, long expertId);

        Task RejectOfferAsync(long scoringId, long areaId, long expertId);

        Task UpdateOffersAsync(Guid projectExternalId);

        Task<Domain.Entities.Scoring> GetByProjectIdAsync(long projectId);

        Task<IReadOnlyCollection<ScoringOfferDetails>> QueryOffersAsync(OffersQuery query, DateTimeOffset now);

        Task<int> GetOffersQueryCountAsync(OffersQuery query, DateTimeOffset now);
    }
}