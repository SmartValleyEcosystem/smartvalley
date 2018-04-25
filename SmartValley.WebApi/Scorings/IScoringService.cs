using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
using SmartValley.WebApi.Projects;
using SmartValley.WebApi.Scorings.Requests;

namespace SmartValley.WebApi.Scorings
{
    public interface IScoringService
    {
        Task<ScoringOffer> GetOfferAsync(long projectId, AreaType areaType, long expertId);

        Task<IReadOnlyCollection<ScoringProjectDetailsWithCounts>> GetScoringProjectsAsync(IReadOnlyCollection<ScoringProjectStatus> statuses);

        Task AcceptOfferAsync(long scoringId, long areaId, long expertId);

        Task RejectOfferAsync(long scoringId, long areaId, long expertId);

        Task UpdateOffersAsync(Guid projectExternalId);

        Task<Scoring> GetByProjectIdAsync(long projectId);

        Task<IReadOnlyCollection<ScoringOfferDetails>> QueryOffersAsync(OffersQuery query, DateTimeOffset now);

        Task<int> GetOffersQueryCountAsync(OffersQuery query, DateTimeOffset now);
    }
}