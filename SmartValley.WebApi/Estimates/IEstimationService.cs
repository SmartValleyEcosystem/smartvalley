using System.Threading.Tasks;

namespace SmartValley.WebApi.Estimates
{
    public interface IEstimationService
    {
        Task SubmitEstimatesAsync(SubmitEstimatesRequest request);
    }
}