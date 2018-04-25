using System.IO;
using System.Threading.Tasks;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Application.Templates
{
    public class TemplateProvider : ITemplateProvider
    {
        private readonly string _contentRootPath;

        public TemplateProvider(string contentRootPath)
        {
            _contentRootPath = contentRootPath;
        }

        public async Task<string> GetEmailTemplateAsync()
        {
            using (var reader = File.OpenText(_contentRootPath + "/email/template.html"))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}