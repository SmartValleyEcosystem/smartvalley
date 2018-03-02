using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.WebApi.Projects;
using SmartValley.WebApi.Scoring.Requests;

namespace SmartValley.WebApi.Scoring
{
    public interface IScoringService
    {
        Task StartAsync(Guid projectExternalId, IReadOnlyCollection<AreaRequest> areas);

        Task<IReadOnlyCollection<ScoringProjectDetailsWithCounts>> GetScoringProjectsAsync(IEnumerable<ScoringProjectStatus> statuses);
    }
}