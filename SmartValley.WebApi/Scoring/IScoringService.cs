using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Scoring
{
    public interface IScoringService
    {
        Task<IReadOnlyCollection<ProjectScoring>> GetProjectsForScoringAsync(ExpertiseArea expertiseArea, string expertAddress);

        Task<IReadOnlyCollection<ProjectScoring>> GetProjectsByAuthorAsync(string authorAddress);

        Task StartAsync(Guid projectExternalId, string transactionHash);
    }
}