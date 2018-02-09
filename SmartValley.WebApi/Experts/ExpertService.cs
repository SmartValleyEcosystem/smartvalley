using System.Linq;
using System.Threading.Tasks;
using SmartValley.Application;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Experts.Requests;
using System.Collections.Generic;
using System;

namespace SmartValley.WebApi.Experts
{
    // TODO 
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

        // TODO 
        List<MockExpertStatus> statuses = new List<MockExpertStatus>
                                          {
                                              new MockExpertStatus{Address = "0xb5662f4eD85C0a3eFc0704A592D487e9C50E3C67", IsConfirmed = false },
                                              new MockExpertStatus{Address = "0xC2aC648a4d834576bEC79C94701560e677e21bd0", IsConfirmed = true }
                                          };

        public ExpertService(EthereumClient ethereumClient, IUserRepository userRepository, IExpertApplicationRepository expertApplicationRepository)
        {
            _ethereumClient = ethereumClient;
            _userRepository = userRepository;
            _expertApplicationRepository = expertApplicationRepository;
        }

        public async Task CreateApplicationAsync(ExpertApplicationRequest request)
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
        }

        // TODO 
        public Task<bool> IsAppliedAsync(string address)
            => Task.FromResult(statuses.Any(i => i.Address.Equals(address, StringComparison.OrdinalIgnoreCase)));

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