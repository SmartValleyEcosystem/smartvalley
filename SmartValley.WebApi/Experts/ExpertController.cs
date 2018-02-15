using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using SmartValley.Application.AzureStorage;
using SmartValley.Domain.Exceptions;
using SmartValley.WebApi.Applications.Requests;
using SmartValley.WebApi.Experts.Requests;
using SmartValley.WebApi.Experts.Responses;

namespace SmartValley.WebApi.Experts
{
    [Route("api/experts")]
    public class ExpertController : Controller
    {
        private readonly IExpertService _expertService;
        private const int FileSizeLimitBytes = 5242880;

        public ExpertController(IExpertService expertService, ExpertApplicationsStorageProvider expertApplicationsStorageProvider)
        {
            _expertService = expertService;
        }

        [HttpGet]
        [Route("{address}/status")]
        public async Task<GetExpertStatusResponse> GetExpertStatusAsync(string address)
        {
            var isApplied = await _expertService.IsAppliedAsync(address);
            var isConfirmed = await _expertService.IsConfirmedAsync(address);
            return new GetExpertStatusResponse {IsConfirmed = isConfirmed, IsApplied = isApplied};
        }

        [HttpPost, DisableRequestSizeLimit, Route("application")]
        public async Task<EmptyResponse> CreateExpertApplicationAsync([FromForm] ExpertApplicationRequest request,
                                                                      IFormFile scan,
                                                                      IFormFile photo,
                                                                      IFormFile cv)
        {
            if (scan == null ||
                photo == null ||
                cv == null ||
                scan.Length < 0 ||
                scan.Length > FileSizeLimitBytes ||
                photo.Length < 0 ||
                photo.Length > FileSizeLimitBytes ||
                cv.Length < 0 ||
                cv.Length > FileSizeLimitBytes)
                throw new AppErrorException(ErrorCode.InvalidFileUploaded);

            await _expertService.CreateApplicationAsync(request, CreateAzureFile(cv), CreateAzureFile(scan), CreateAzureFile(photo));

            return new EmptyResponse();
        }

        private AzureFile CreateAzureFile(IFormFile formFile)
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
    }
}