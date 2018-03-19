using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;
using SmartValley.WebApi.Applications.Requests;

namespace SmartValley.WebApi.Applications
{
    public interface IApplicationService
    {
        Task CreateAsync(ApplicationRequest applicationRequest);

        Task<IReadOnlyCollection<Country>> GetCountriesAsync();

        Task<IReadOnlyCollection<Category>> GetCategoriesAsync();

        Task<IReadOnlyCollection<Stage>> GetStagesAsync();

        Task<IReadOnlyCollection<SocialMedia>> GetSocialMediasAsync();
    }
}