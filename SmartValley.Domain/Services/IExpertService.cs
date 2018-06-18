using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Services
{
    public interface IExpertService
    {
        Task<Expert> GetByAddressAsync(string address);

        Task UpdateExpertAreasAsync(long expertId);
    }
}