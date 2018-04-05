using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;
using SmartValley.WebApi.ScoringApplications.Requests;

namespace SmartValley.WebApi.ScoringApplications
{
    public interface IScoringApplicationService
    {
        Task<IReadOnlyCollection<ScoringApplicationQuestion>> GetQuestionsAsync();

        Task<Domain.ScoringApplication> GetApplicationAsync(long projectId);

        Task SaveApplicationAsync(long projectId, SaveScoringApplicationRequest request);

        Task SubmitApplicationAsync(long projectId);
    }
}