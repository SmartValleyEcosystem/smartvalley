using System.Linq;
using System.Threading.Tasks;
using SmartValley.Application;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Experts.Requests;
using System.Collections.Generic;
using System;
using System.IO;
using SmartValley.Application.AzureStorage;

namespace SmartValley.WebApi.Experts
{
    class MockExpertStatus
    {
        public string Address { get; set; }
        public bool IsConfirmed { get; set; }
    }

    public class ExpertService : IExpertService
    {
        private readonly EthereumClient _ethereumClient;
        private readonly IUserRepository _userRepository;
        private readonly IExpertApplicationRepository _expertApplicationRepository;
        private readonly ExpertApplicationsStorageProvider _expertApplicationsStorageProvider;

        // TODO 
        List<MockExpertStatus> statuses = new List<MockExpertStatus>
                                          {
                                              new MockExpertStatus {Address = "0xb5662f4eD85C0a3eFc0704A592D487e9C50E3C67", IsConfirmed = false},
                                              new MockExpertStatus {Address = "0xC2aC648a4d834576bEC79C94701560e677e21bd0", IsConfirmed = true}
                                          };

        public ExpertService(EthereumClient ethereumClient,
                             IUserRepository userRepository,
                             IExpertApplicationRepository expertApplicationRepository,
                             ExpertApplicationsStorageProvider expertApplicationsStorageProvider)
        {
            _ethereumClient = ethereumClient;
            _userRepository = userRepository;
            _expertApplicationRepository = expertApplicationRepository;
            _expertApplicationsStorageProvider = expertApplicationsStorageProvider;
        }

        public async Task CreateApplicationAsync(ExpertApplicationRequest request, AzureFile cv, AzureFile scan, AzureFile photo)
        {
            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);

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

        // TODO 
        public Task<bool> IsConfirmedAsync(string address)
        {
            return Task.Run(() =>
                            {
                                var status = statuses.FirstOrDefault(i => i.Address.Equals(address, StringComparison.OrdinalIgnoreCase));
                                if (status == null)
                                    return false;
                                return status.IsConfirmed;
                            });
        }
    }
}