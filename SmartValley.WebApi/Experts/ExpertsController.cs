using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Application.Extensions;
using SmartValley.Domain;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;
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
        private readonly ICountryRepository _countryRepository;
        private readonly IUserRepository _userRepository;

        public ExpertsController(
            IExpertService expertService,
            EthereumClient ethereumClient,
            ICountryRepository countryRepository,
            IUserRepository userRepository)
        {
            _ethereumClient = ethereumClient;
            _expertService = expertService;
            _ethereumClient = ethereumClient;
            _countryRepository = countryRepository;
            _userRepository = userRepository;
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

        [HttpGet("{address}")]
        public async Task<ExpertResponse> GetExpertAsync(string address)
        {
            var expert = await _expertService.GetByAddressAsync(address);
            return ExpertResponse.Create(expert);
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
            var application = await _expertService.GetApplicationByIdAsync(id);
            if (application == null)
                throw new AppErrorException(ErrorCode.ApplicationNotFound);

            var country = await _countryRepository.GetByIdAsync(application.CountryId);
            if (country == null)
                throw new AppErrorException(ErrorCode.CountryNotFound);

            var applicant = await _userRepository.GetByIdAsync(application.ApplicantId);
            if (applicant == null)
                throw new AppErrorException(ErrorCode.UserNotFound);

            return ExpertApplicationResponse.Create(application, applicant, country);
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

        [HttpDelete]
        [Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<IActionResult> DeleteExpertAsync([FromBody] ExpertDeleteRequest request)
        {
            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);
            await _expertService.DeleteAsync(request.Address);
            return NoContent();
        }

        [HttpGet]
        [Authorize(Roles = nameof(RoleType.Admin))]
        public async Task<PartialCollectionResponse<ExpertResponse>> GetAsync(QueryExpertsRequest request)
        {
            var query = new ExpertsQuery
                        {
                            IsInHouse = request.IsInHouse,
                            Count = request.Count,
                            Offset = request.Offset
                        };
            var experts = await _expertService.GetAsync(query);
            return experts.ToPartialCollectionResponse(ExpertResponse.Create);
        }

        [HttpGet("availability")]
        [Authorize(Roles = nameof(RoleType.Expert))]
        public async Task<ExpertAvailabilityResponse> GetAvailabilityAsync()
        {
            var expert = await _expertService.GetByIdAsync(User.GetUserId());
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
        public async Task<EmptyResponse> CreateExpertApplicationAsync(
            [FromForm] CreateExpertApplicationRequest request,
            IFormFile scan,
            IFormFile photo,
            IFormFile cv)
        {
            if (!scan.IsImageValid() || !photo.IsImageValid() || !cv.IsCvValid())
                throw new AppErrorException(ErrorCode.InvalidFileUploaded);

            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);
            await _expertService.CreateApplicationAsync(request, User.GetUserId(), cv.ToAzureFile(), scan.ToAzureFile(), photo.ToAzureFile());

            return new EmptyResponse();
        }
    }
}