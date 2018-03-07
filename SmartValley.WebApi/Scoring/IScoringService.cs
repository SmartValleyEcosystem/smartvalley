using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain;
using SmartValley.WebApi.Projects;
using SmartValley.WebApi.Scoring.Requests;

namespace SmartValley.WebApi.Scoring
{
    public interface IScoringService
    {
        Task StartAsync(Guid projectExternalId, IReadOnlyCollection<AreaRequest> areas);

        Task<IReadOnlyCollection<ScoringProjectDetailsWithCounts>> GetScoringProjectsAsync(IReadOnlyCollection<ScoringProjectStatus> statuses);

        Task<IReadOnlyCollection<ScoringOfferDetails>> GetPendingOfferDetailsAsync(string expertAddress);

        Task<IReadOnlyCollection<ScoringOfferDetails>> GetAcceptedOfferDetailsAsync(string expertAddress);

        Task AcceptOfferAsync(long scoringId, long areaId, string expertAddress);

        Task RejectOfferAsync(long scoringId, long areaId, string expertAddress);
    }
}