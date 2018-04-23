using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using SmartValley.Application.AzureStorage;

namespace SmartValley.Application.Extensions
{
    public static class FormFileExtensions
    {
        private static readonly string[] PhotoExtensions = { ".jpeg", ".jpg", ".png" };
        private const int FileSizeLimitBytes = 5242880;

        public static AzureFile ToAzureFile(this IFormFile formFile)
        {
            using (var stream = formFile.OpenReadStream())
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return new AzureFile(formFile.FileName, memoryStream.GetBuffer());
                }
            }
        }

        public static bool IsImageValid(this IFormFile file)
        {
            return file.IsValid() && PhotoExtensions.Contains(Path.GetExtension(file.FileName).ToLower())
                   && file.ContentType.ToLower().Contains("image");
        }

        public static bool IsValid(this IFormFile file)
        {
            return file != null && file.Length < FileSizeLimitBytes;
        }
    }
}