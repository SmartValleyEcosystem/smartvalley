using System.Threading.Tasks;
using SmartValley.Application;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Experts.Requests;
using System.Collections.Generic;
using SmartValley.Application.AzureStorage;
using SmartValley.Domain;

namespace SmartValley.WebApi.Experts
{
    public class ExpertService : IExpertService
    {
        private readonly IUserRepository _userRepository;
        private readonly IExpertRepository _expertRepository;
        private readonly IExpertApplicationRepository _expertApplicationRepository;
        private readonly ExpertApplicationsStorageProvider _expertApplicationsStorageProvider;

        public ExpertService(EthereumClient ethereumClient,
                             IUserRepository userRepository,
                             IExpertApplicationRepository expertApplicationRepository,
                             ExpertApplicationsStorageProvider expertApplicationsStorageProvider,
                             IExpertRepository expertRepository)
        {
            _userRepository = userRepository;
            _expertRepository = expertRepository;
            _expertApplicationRepository = expertApplicationRepository;
            _expertApplicationsStorageProvider = expertApplicationsStorageProvider;
        }

        public async Task CreateApplicationAsync(ExpertApplicationRequest request, AzureFile cv, AzureFile scan, AzureFile photo)
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
                DocumentType = request.DocumentType.ToDomain()
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

        public Task<IReadOnlyCollection<ExpertDetails>> GetAllExpertsDetailsAsync()
            => _expertRepository.GetAllDetailsAsync();

        public Task<bool> IsExpertAsync(string address)
            => _userRepository.HasRoleAsync(address, RoleType.Expert);
    }
}