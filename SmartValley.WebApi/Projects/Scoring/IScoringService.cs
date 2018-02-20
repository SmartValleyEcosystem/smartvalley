using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;
using SmartValley.WebApi.Projects.Scoring.Requests;

namespace SmartValley.WebApi.Projects.Scoring
{
    public interface IScoringService
    {
        Task StartAsync(Guid projectExternalId, IReadOnlyCollection<AreaRequest> areas);
    }
}