using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;
using SmartValley.WebApi.ScoringApplication.Requests;

namespace SmartValley.WebApi.ScoringApplication
{
    public interface IScoringApplicationService
    {
        Task<IReadOnlyCollection<ScoringApplicationQuestion>> GetQuestionsAsync();

        Task<Domain.ScoringApplication> GetApplicationAsync(long projectId);

        Task SaveApplicationAsync(long projectId, SaveScoringApplicationRequest saveScoringApplicationRequest);

        Task SubmitForScoreAsync(long projectId);
    }
}