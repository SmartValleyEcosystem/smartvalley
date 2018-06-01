using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Application.AzureStorage;
using SmartValley.Application.Email;
using SmartValley.Domain;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Experts.Requests;

namespace SmartValley.WebApi.Experts
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ExpertService : IExpertService
    {
        private readonly IUserRepository _userRepository;
        private readonly IExpertRepository _expertRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly MailService _mailService;
        private readonly IExpertApplicationRepository _expertApplicationRepository;
        private readonly ExpertApplicationsStorageProvider _expertApplicationsStorageProvider;
        private readonly IClock _clock;

        public ExpertService(IUserRepository userRepository,
                             IExpertApplicationRepository expertApplicationRepository,
                             IClock clock,
                             ICountryRepository countryRepository,
                             ExpertApplicationsStorageProvider expertApplicationsStorageProvider,
                             IExpertRepository expertRepository,
                             MailService mailService)
        {
            _userRepository = userRepository;
            _expertRepository = expertRepository;
            _countryRepository = countryRepository;
            _mailService = mailService;
            _expertApplicationRepository = expertApplicationRepository;
            _expertApplicationsStorageProvider = expertApplicationsStorageProvider;
            _clock = clock;
        }

        public async Task CreateApplicationAsync(CreateExpertApplicationRequest request, long userId, AzureFile cv, AzureFile scan, AzureFile photo)
        {
            var country = await GetCountryAsync(request.CountryIsoCode);

            var expertApplication = new ExpertApplication
                                    {
                                        FirstName = request.FirstName,
                                        LastName = request.LastName,
                                        Sex = request.Sex,
                                        ApplicantId = userId,
                                        BirthDate = request.BirthDate,
                                        CountryId = country.Id,
                                        City = request.City,
                                        Description = request.Description,
                                        Why = request.Why,
                                        FacebookLink = request.FacebookLink,
                                        LinkedInLink = request.LinkedInLink,
                                        BitcointalkLink = request.BitcointalkLink,
                                        DocumentNumber = request.DocumentNumber,
                                        DocumentType = request.DocumentType.ToDomain(),
                                        ApplyDate = _clock.UtcNow
                                    };

            expertApplication.SetAreas(request.Areas);

            _expertApplicationRepository.Add(expertApplication);

            await _expertApplicationRepository.SaveChangesAsync();

            var applicationId = expertApplication.Id.ToString();

            var scanName = $"application-{applicationId}/scan-{Guid.NewGuid()}{scan.Extension}";
            var photoName = $"application-{applicationId}/photo-{Guid.NewGuid()}{photo.Extension}";
            var cvName = $"application-{applicationId}/cv-{Guid.NewGuid()}{cv.Extension}";

            var links = await Task.WhenAll(_expertApplicationsStorageProvider.UploadAndGetUriAsync(scanName, scan),
                                           _expertApplicationsStorageProvider.UploadAndGetUriAsync(cvName, cv),
                                           _expertApplicationsStorageProvider.UploadAndGetUriAsync(photoName, photo));

            expertApplication.ScanUrl = links[0];
            expertApplication.CvUrl = links[1];
            expertApplication.PhotoUrl = links[2];

            await _expertApplicationRepository.SaveChangesAsync();
        }

        public Task<ExpertApplication> GetApplicationByIdAsync(long id)
            => _expertApplicationRepository.GetByIdAsync(id);

        public Task<IReadOnlyCollection<ExpertApplication>> GetPendingApplicationsAsync()
            => _expertApplicationRepository.GetAllByStatusAsync(ExpertApplicationStatus.Pending);

        public async Task<ExpertApplicationStatus> GetExpertApplicationStatusAsync(Address address)
        {
            var user = await _userRepository.GetByAddressAsync(address);
            if (user == null)
                throw new AppErrorException(ErrorCode.UserNotFound);

            var application = await _expertApplicationRepository.GetByUserIdAsync(user.Id);

            return application?.Status ?? ExpertApplicationStatus.None;
        }

        public Task<Expert> GetByIdAsync(long expertId)
            => _expertRepository.GetByIdAsync(expertId);

        public Task<Expert> GetByAddressAsync(Address address)
            => _expertRepository.GetByAddressAsync(address);

        public async Task SetAvailabilityAsync(Address address, bool isAvailable)
        {
            var expert = await _expertRepository.GetByAddressAsync(address);
            if (expert == null)
                throw new AppErrorException(ErrorCode.ExpertNotFound);

            expert.IsAvailable = isAvailable;
            await _expertRepository.SaveChangesAsync();
        }

        public async Task SetInHouseAsync(Address address, bool isInHouse)
        {
            var expert = await _expertRepository.GetByAddressAsync(address);
            if (expert == null)
                throw new AppErrorException(ErrorCode.ExpertNotFound);

            expert.IsInHouse = isInHouse;
            await _expertRepository.SaveChangesAsync();
        }

        public async Task AddAsync(ExpertRequest request)
        {
            var user = await _userRepository.GetByAddressAsync(request.Address);
            if (user == null)
                throw new AppErrorException(ErrorCode.UserNotFound);

            user.SecondName = request.SecondName;
            user.FirstName = request.FirstName;
            user.Email = request.Email;
            user.About = request.About;

            await _userRepository.SaveChangesAsync();
            await _userRepository.AddRoleAsync(request.Address, RoleType.Expert);

            var expert = new Expert(user.Id, true);
            expert.SetAreas(request.Areas);

            _expertRepository.Add(expert);

            await _expertRepository.SaveChangesAsync();
        }

        public async Task UpdateAreasAsync(Address address, IReadOnlyCollection<int> areas)
        {
            var expert = await _expertRepository.GetByAddressAsync(address);
            if (expert == null)
                throw new AppErrorException(ErrorCode.ExpertNotFound);

            expert.SetAreas(areas);
            await _expertRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(Address address)
        {
            await _userRepository.RemoveRoleAsync(address, RoleType.Expert);

            var expert = await _expertRepository.GetByAddressAsync(address);
            _expertRepository.Remove(expert);
            await _expertRepository.SaveChangesAsync();
        }

        public async Task AcceptApplicationAsync(long id, IReadOnlyCollection<int> areas)
        {
            var application = await _expertApplicationRepository.GetByIdAsync(id);
            if (application.Status != ExpertApplicationStatus.Pending)
                throw new AppErrorException(ErrorCode.ExpertApplicationAlreadyProcessed);

            var user = await _userRepository.GetByIdAsync(application.ApplicantId);
            if (user == null)
                throw new AppErrorException(ErrorCode.UserNotFound);

            user.FirstName = application.FirstName;
            user.SecondName = application.LastName;
            user.CountryId = application.CountryId;
            user.BitcointalkLink = application.BitcointalkLink;
            user.City = application.City;
            user.FacebookLink = application.FacebookLink;
            user.LinkedInLink = application.LinkedInLink;
            user.BirthDate = application.BirthDate;
            user.Sex = application.Sex;

            await _userRepository.SaveChangesAsync();

            var expert = await _expertRepository.GetByAddressAsync(user.Address);
            if (expert == null)
            {
                expert = new Expert(user.Id, true);
                _expertRepository.Add(expert);
            }

            expert.SetAreas(areas);
            await _expertRepository.SaveChangesAsync();

            application.SetAccepted(areas);

            await _expertApplicationRepository.SaveChangesAsync();
            await _mailService.SendExpertApplicationAcceptedAsync(user.Email, application.FirstName);
        }

        public async Task RejectApplicationAsync(long id)
        {
            var application = await _expertApplicationRepository.GetByIdAsync(id);
            if (application.Status != ExpertApplicationStatus.Pending)
                throw new AppErrorException(ErrorCode.ExpertApplicationAlreadyProcessed);

            application.SetRejected();

            await _expertApplicationRepository.SaveChangesAsync();

            var user = await _userRepository.GetByIdAsync(application.ApplicantId);
            if (user == null)
                throw new AppErrorException(ErrorCode.UserNotFound);

            await _mailService.SendExpertApplicationRejectedAsync(user.Email, application.FirstName);
        }

        public Task<PagingCollection<Expert>> GetAsync(ExpertsQuery query)
            => _expertRepository.GetAsync(query);

        public Task<IReadOnlyCollection<Area>> GetAreasAsync()
            => _expertRepository.GetAreasAsync();

        private async Task<Country> GetCountryAsync(string code)
            => await _countryRepository.GetByCodeAsync(code) ?? throw new AppErrorException(ErrorCode.CountryNotFound);
    }
}