using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface ICountryRepository
    {
        Task<Country> GetByCodeAsync(string code);

        Task<Country> GetByIdAsync(long id);
    }
}