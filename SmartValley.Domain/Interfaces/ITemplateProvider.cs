using System.Threading.Tasks;

namespace SmartValley.Domain.Interfaces
{
    public interface ITemplateProvider
    {
        Task<string> GetEmailTemplateAsync();
    }
}