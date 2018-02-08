using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Application.Templates
{
    public class TemplateProvider : ITemplateProvider
    {
        private readonly IHostingEnvironment _environment;

        public TemplateProvider(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> GetEmailTemplateAsync()
        {
            using (var reader = File.OpenText(_environment.ContentRootPath + "/email/template.html"))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}