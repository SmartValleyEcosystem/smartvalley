using System.Threading.Tasks;
using SmartValley.Application;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Experts.Requests;
using System.Collections.Generic;
using System.Linq;
using SmartValley.Application.AzureStorage;
using SmartValley.Domain;
using SmartValley.Domain.Exceptions;

namespace SmartValley.WebApi.Experts
{
    public class ExpertService : IExpertService
    {
        private readonly IUserRepository _userRepository;
        private readonly IExpertRepository _expertRepository;
        private readonly IExpertApplicationRepository _expertApplicationRepository;
        private readonly ExpertApplicationsStorageProvider _expertApplicationsStorageProvider;
        private readonly IClock _clock;

        public ExpertService(EthereumClient ethereumClient,
                             IUserRepository userRepository,
                             IExpertApplicationRepository expertApplicationRepository,
                             IClock clock,
                             ExpertApplicationsStorageProvider expertApplicationsStorageProvider,
                             IExpertRepository expertRepository)
        {
            _userRepository = userRepository;
            _expertRepository = expertRepository;
            _expertApplicationRepository = expertApplicationRepository;
            _expertApplicationsStorageProvider = expertApplicationsStorageProvider;
            _clock = clock;
        }

        public async Task CreateApplicationAsync(CreateExpertApplicationRequest request, AzureFile cv, AzureFile scan, AzureFile photo)
        {
            var user = await _userRepository.GetByAddressAsync(request.ApplicantAddress);

            var expertApplication = new ExpertApplication
                                    {
                                        FirstName = request.FirstName,
                                        LastName = request.LastName,
                                        Sex = request.Sex,
                                        ApplicantId = user.Id,
                                        BirthDate = request.BirthDate,
                                        Country = request.CountryIsoCode,
                                        City = request.City,
                                        Description = request.Description,
                                        Why = request.Why,
                                        FacebookLink = request.FacebookLink,
                                        LinkedInLink = request.LinkedInLink,
                                        DocumentNumber = request.DocumentNumber,
                                        DocumentType = request.DocumentType.ToDomain(),
                                        ApplyDate = _clock.UtcNow
            };

            await _expertApplicationRepository.AddAsync(expertApplication, request.Areas);

            var applicationId = expertApplication.Id.ToString();

            var scanName = $"application-{applicationId}/scan{scan.Extension}";
            var photoName = $"application-{applicationId}/photo{photo.Extension}";
            var cvName = $"application-{applicationId}/cv{cv.Extension}";

            await Task.WhenAll(_expertApplicationsStorageProvider.UploadAsync(scanName, scan),
                               _expertApplicationsStorageProvider.UploadAsync(cvName, cv),
                               _expertApplicationsStorageProvider.UploadAsync(photoName, photo));

            expertApplication.ScanName = scanName;
            expertApplication.CvName = cvName;
            expertApplication.PhotoName = photoName;

            await _expertApplicationRepository.UpdateWholeAsync(expertApplication);
        }

        public Task<ExpertApplicationDetails> GetApplicationByIdAsync(long id)
        {
            return _expertApplicationRepository.GetDetailsByIdAsync(id);
        }

        public Task<IReadOnlyCollection<ExpertApplication>> GetPendingApplicationsAsync()
        {
            return _expertApplicationRepository.GetAllByStatusAsync(ExpertApplicationStatus.Pending);
        }

        public Task<bool> IsAppliedAsync(string address)
            => _expertApplicationRepository.IsAppliedAsync(address);
        
        public async Task<bool> IsConfirmedAsync(string address)
        {
            var existExpert = await _expertRepository.GetByAddressAsync(address);
            return existExpert != null;
        }

        public async Task AddAsync(string address)
        {
            await _userRepository.AddRoleAsync(address, RoleType.Expert);
            var user = await _userRepository.GetByAddressAsync(address);
            await _expertRepository.AddAsync(new Expert { IsAvailable = true, UserId = user.Id });
        }

        public async Task DeleteAsync(string address)
        {
            await _userRepository.RemoveRoleAsync(address, RoleType.Expert);
            var user = await _userRepository.GetByAddressAsync(address);
            await _expertRepository.RemoveAsync(new Expert { IsAvailable = true, UserId = user.Id });
        }

        public async Task AcceptApplicationAsync(long id, IReadOnlyCollection<int> areas)
        {
            var application = await _expertApplicationRepository.GetDetailsByIdAsync(id);
            if (application.ExpertApplication.Status != ExpertApplicationStatus.Pending)
                throw new AppErrorException(ErrorCode.ExpertApplicationAlreadyProcessed);

            await _expertApplicationRepository.SetAcceptedAsync(application, areas.ToList());
        }

        public async Task RejectApplicationAsync(long id)
        {
            var application = await _expertApplicationRepository.GetDetailsByIdAsync(id);
            if (application.ExpertApplication.Status != ExpertApplicationStatus.Pending)
                throw new AppErrorException(ErrorCode.ExpertApplicationAlreadyProcessed);

            await _expertApplicationRepository.SetRejectedAsync(application);
        }

        public Task<IReadOnlyCollection<ExpertDetails>> GetAllExpertsDetailsAsync()
            => _expertRepository.GetAllDetailsAsync();

        public Task<bool> IsExpertAsync(string address)
            => _userRepository.HasRoleAsync(address, RoleType.Expert);
    }
}