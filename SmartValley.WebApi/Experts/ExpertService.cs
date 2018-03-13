using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Application.AzureStorage;
using SmartValley.Domain;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Experts.Requests;

namespace SmartValley.WebApi.Experts
{
    public class ExpertService : IExpertService
    {
        private readonly IUserRepository _userRepository;
        private readonly IExpertRepository _expertRepository;
        private readonly IExpertApplicationRepository _expertApplicationRepository;
        private readonly ExpertApplicationsStorageProvider _expertApplicationsStorageProvider;
        private readonly IClock _clock;

        public ExpertService(IUserRepository userRepository,
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

        public Task<ExpertApplicationStatus> GetExpertApplicationStatusAsync(Address address)
            => _expertApplicationRepository.GetExpertApplicationStatusAsync(address);

        public async Task AddAsync(ExpertRequest request)
        {
            var user = await _userRepository.GetByAddressAsync(request.Address);
            if (user == null)
                throw new AppErrorException(ErrorCode.UserNotFound);

            user.About = request.About;
            user.Name = request.Name;
            user.Email = request.Email;

            await _userRepository.UpdateWholeAsync(user);
            await _userRepository.AddRoleAsync(request.Address, RoleType.Expert);
            await _expertRepository.AddAsync(new Expert
                                             {
                                                 IsAvailable = true,
                                                 UserId = user.Id
                                             }, request.Areas);
        }

        public async Task UpdateAsync(UpdateExpertRequest request)
        {
            var user = await _userRepository.GetByAddressAsync(request.Address);
            if (user == null)
                throw new AppErrorException(ErrorCode.UserNotFound);

            user.About = request.About;
            user.Name = request.Name;
            user.Email = request.Email;

            await _userRepository.UpdateWholeAsync(user);
            await _expertRepository.UpdateAsync(new Expert
                                                {
                                                    IsAvailable = request.IsAvailable,
                                                    UserId = user.Id
                                                }, request.Areas);
        }

        public async Task DeleteAsync(Address address)
        {
            await _userRepository.RemoveRoleAsync(address, RoleType.Expert);
            var user = await _userRepository.GetByAddressAsync(address);
            await _expertRepository.RemoveAsync(new Expert {IsAvailable = true, UserId = user.Id});
        }

        public async Task AcceptApplicationAsync(long id, IReadOnlyCollection<int> areas)
        {
            var application = await _expertApplicationRepository.GetDetailsByIdAsync(id);
            if (application.ExpertApplication.Status != ExpertApplicationStatus.Pending)
                throw new AppErrorException(ErrorCode.ExpertApplicationAlreadyProcessed);

            var user = await _userRepository.GetByIdAsync(application.ExpertApplication.ApplicantId);
            if (user == null)
                throw new AppErrorException(ErrorCode.UserNotFound);

            user.Name = $"{application.ExpertApplication.LastName} {application.ExpertApplication.FirstName}";
            await _userRepository.UpdateWholeAsync(user);

            var expert = await _expertRepository.GetByAddressAsync(user.Address);
            if (expert != null)
            {
                await _expertRepository.UpdateAsync(expert, areas);
            }
            else
            {
                await _expertRepository.AddAsync(new Expert
                                                 {
                                                     UserId = user.Id,
                                                     IsAvailable = true
                                                 }, areas);
            }

            await _expertApplicationRepository.SetAcceptedAsync(application, areas.ToList());
        }

        public async Task RejectApplicationAsync(long id)
        {
            var application = await _expertApplicationRepository.GetDetailsByIdAsync(id);
            if (application.ExpertApplication.Status != ExpertApplicationStatus.Pending)
                throw new AppErrorException(ErrorCode.ExpertApplicationAlreadyProcessed);

            await _expertApplicationRepository.SetRejectedAsync(application);
        }

        public Task<PagingList<ExpertDetails>> GetAllExpertsDetailsAsync(int page, int pageSize)
            => _expertRepository.GetAllDetailsAsync(page, pageSize);

        public Task<bool> IsExpertAsync(Address address)
            => _userRepository.HasRoleAsync(address, RoleType.Expert);

        public Task<IReadOnlyCollection<Area>> GetAreasAsync()
            => _expertRepository.GetAreasAsync();
    }
}