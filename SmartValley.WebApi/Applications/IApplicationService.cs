using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;
using SmartValley.WebApi.Applications.Requests;

namespace SmartValley.WebApi.Applications
{
    public interface IApplicationService
    {
        Task CreateAsync(long userId, ApplicationRequest applicationRequest);

        Task<IReadOnlyCollection<Country>> GetCountriesAsync();
    }
}