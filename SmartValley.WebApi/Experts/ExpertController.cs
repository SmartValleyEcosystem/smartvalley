using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Application;
using SmartValley.Application.AzureStorage;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.WebApi.Experts.Requests;
using SmartValley.WebApi.Experts.Responses;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Experts
{
    [Route("api/experts")]
    public class ExpertController : Controller
    {
        private readonly IExpertService _expertService;
        private readonly EthereumClient _ethereumClient;
        private const int FileSizeLimitBytes = 5242880;

        public ExpertController(
            IExpertService expertService,
            ExpertApplicationsStorageProvider expertApplicationsStorageProvider,
            EthereumClient ethereumClient)
        {
            _ethereumClient = ethereumClient;
            _expertService = expertService;
            _ethereumClient = ethereumClient;
        }

        [HttpGet, Route("areas")]
        public async Task<CollectionResponse<AreaResponse>> GetAreasAsync()
        {
            var areas = await _expertService.GetAreasAsync();
            return new CollectionResponse<AreaResponse>
                   {
                       Items = areas.Select(AreaResponse.Create).ToArray()
                   };
        }

        [HttpGet, Route("{address}/status")]
        public async Task<GetExpertStatusResponse> GetExpertStatusAsync(string address)
        {
            var isApplied = await _expertService.IsAppliedAsync(address);
            var isConfirmed = await _expertService.IsConfirmedAsync(address);
            return new GetExpertStatusResponse {IsConfirmed = isConfirmed, IsApplied = isApplied};
        }

        [HttpGet("applications")]
        [Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<CollectionResponse<PendingExpertApplicationsResponse>> GetPendingApplicationsAsync()
        {
            var applications = await _expertService.GetPendingApplicationsAsync();
            return new CollectionResponse<PendingExpertApplicationsResponse>
                   {
                       Items = applications.Select(PendingExpertApplicationsResponse.Create).ToArray()
                   };
        }

        [HttpGet("applications/{id}")]
        [Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<ExpertApplicationResponse> GetApplicationByIdAsync(long id)
        {
            var applicationDetails = await _expertService.GetApplicationByIdAsync(id);
            return ExpertApplicationResponse.Create(applicationDetails);
        }

        [HttpPost("applications/{id}/accept")]
        [Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<EmptyResponse> AcceptAsync(long id, [FromBody] AcceptApplicationRequest request)
        {
            if (request.Areas.Count == 0)
                throw new AppErrorException(ErrorCode.ShouldCheckAreasToAccept);

            await _expertService.AcceptApplicationAsync(id, request.Areas);
            return new EmptyResponse();
        }

        [HttpPost("applications/{id}/reject")]
        [Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<EmptyResponse> RejectAsync(long id, [FromBody] RejectApplicationRequest request)
        {
            await _expertService.RejectApplicationAsync(id);
            return new EmptyResponse();
        }

        [HttpPost]
        [Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<IActionResult> CreateExpertAsync([FromBody] ExpertRequest request)
        {
            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);
            await _expertService.AddAsync(request.Address);
            return NoContent();
        }

        [HttpDelete]
        [Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<IActionResult> DeleteExpertAsync(string address, string transactionHash)
        {
            await _ethereumClient.WaitForConfirmationAsync(transactionHash);
            await _expertService.DeleteAsync(address);
            return NoContent();
        }

        [HttpGet]
        [Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<IActionResult> GetAllExperts()
        {
            var experts = await _expertService.GetAllExpertsDetailsAsync();
            return Ok(new CollectionResponse<ExpertResponse>
                      {
                          Items = experts.Select(i => new ExpertResponse
                                                      {
                                                          Address = i.Address,
                                                          Email = i.Email,
                                                          About = i.About,
                                                          IsAvailable = i.IsAvailable,
                                                          Name = i.Name,
                                                          Areas = i.Areas.Select(j => new AreaResponse {Id = j.Id.FromDomain(), Name = j.Name}).ToArray()
                                                      }).ToArray()
                      });
        }

        [HttpPost, DisableRequestSizeLimit, Route("application")]
        public async Task<EmptyResponse> CreateExpertApplicationAsync([FromForm] CreateExpertApplicationRequest request,
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
            {
                throw new AppErrorException(ErrorCode.InvalidFileUploaded);
            }

            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);
            await _expertService.CreateApplicationAsync(request, CreateAzureFile(cv), CreateAzureFile(scan), CreateAzureFile(photo));

            return new EmptyResponse();
        }

        private static AzureFile CreateAzureFile(IFormFile formFile)
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