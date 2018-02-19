using System;
using System.Threading.Tasks;

namespace SmartValley.WebApi.Projects.Scoring
{
    public interface IScoringService
    {
        Task StartAsync(Guid projectExternalId);
    }
}