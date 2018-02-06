using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Scoring
{
    public interface IScoringService
    {
        Task StartAsync(Guid projectExternalId, string transactionHash);
    }
}