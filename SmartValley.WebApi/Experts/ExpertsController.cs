using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Application;
using SmartValley.Application.Extensions;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.Ethereum;
using SmartValley.WebApi.Experts.Requests;
using SmartValley.WebApi.Experts.Responses;
using SmartValley.WebApi.Extensions;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Experts
{
    [Route("api/experts")]
    public class ExpertsController : Controller
    {
        private readonly IExpertService _expertService;
        private readonly EthereumClient _ethereumClient;

        public ExpertsController(
            IExpertService expertService,
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

        [HttpGet]
        public async Task<ExpertResponse> GetExpertAsync(string address)
        {
            var expertDetails = await _expertService.GetDetailsAsync(address);
            return ExpertResponse.Create(expertDetails);
        }

        [HttpGet, Route("{address}/status")]
        public async Task<GetExpertStatusResponse> GetExpertStatusAsync(string address)
        {
            var status = await _expertService.GetExpertApplicationStatusAsync(address);
            return new GetExpertStatusResponse {Status = status};
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
            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);

            if (request.Areas.Count == 0)
                throw new AppErrorException(ErrorCode.ShouldCheckAreasToAccept);

            await _expertService.AcceptApplicationAsync(id, request.Areas);

            return new EmptyResponse();
        }

        [HttpPost("applications/{id}/reject")]
        [Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<EmptyResponse> RejectAsync(long id, [FromBody] RejectApplicationRequest request)
        {
            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);

            await _expertService.RejectApplicationAsync(id);

            return new EmptyResponse();
        }

        [HttpPost]
        [Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<IActionResult> CreateExpertAsync([FromBody] ExpertRequest request)
        {
            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);
            await _expertService.AddAsync(request);
            return NoContent();
        }

        [HttpPut]
        [Authorize(Roles = nameof(RoleType.Expert))]
        public async Task<IActionResult> UpdateExpertAsync([FromBody] ExpertUpdateRequest request)
        {
            await _expertService.UpdateAsync(User.Identity.Name, request);
            return NoContent();
        }

        [HttpDelete]
        [Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<IActionResult> DeleteExpertAsync([FromBody] ExpertDeleteRequest request)
        {
            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);
            await _expertService.DeleteAsync(request.Address);
            return NoContent();
        }

        [HttpGet("all")]
        [Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<PartialCollectionResponse<ExpertResponse>> GetAllExperts(AllExpertsRequest request)
        {
            var experts = await _expertService.GetAllExpertsDetailsAsync(request.Offset, request.Count);
            var totalCount = await _expertService.GetTotalCountExpertsAsync();
            return new PartialCollectionResponse<ExpertResponse>(
                request.Offset, experts.Count, totalCount, experts.Select(ExpertResponse.Create).ToArray());
        }

        [HttpGet("availability")]
        [Authorize(Roles = nameof(RoleType.Expert))]
        public async Task<ExpertAvailabilityResponse> GetAvailabilityAsync()
        {
            var expert = await _expertService.GetAsync(User.GetUserId());
            if (expert == null)
                throw new AppErrorException(ErrorCode.ExpertNotFound);

            return new ExpertAvailabilityResponse {IsAvailable = expert.IsAvailable};
        }

        [HttpPut("availability")]
        [Authorize(Roles = nameof(RoleType.Expert))]
        public async Task<IActionResult> SetAvailabilityAsync([FromBody] SetAvailabilityRequest request)
        {
            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);

            await _expertService.SetAvailabilityAsync(User.Identity.Name, request.Value);
            return NoContent();
        }

        [HttpPost, DisableRequestSizeLimit, Route("applications")]
        public async Task<EmptyResponse> CreateExpertApplicationAsync([FromForm] CreateExpertApplicationRequest request,
                                                                      IFormFile scan,
                                                                      IFormFile photo,
                                                                      IFormFile cv)
        {
            if (!scan.IsImageValid()
                || !photo.IsImageValid()
                || !cv.IsCVValid())
            {
                throw new AppErrorException(ErrorCode.InvalidFileUploaded);
            }

            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);
            await _expertService.CreateApplicationAsync(request, User.GetUserId(), cv.ToAzureFile(), scan.ToAzureFile(), photo.ToAzureFile());

            return new EmptyResponse();
        }
    }
}