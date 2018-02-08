using System.Linq;
using System.Threading.Tasks;
using SmartValley.Application;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Experts.Requests;

namespace SmartValley.WebApi.Experts
{
    public class ExpertService : IExpertService
    {
        private readonly EthereumClient _ethereumClient;
        private readonly IUserRepository _userRepository;
        private readonly IExpertApplicationRepository _expertApplicationRepository;

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
    }
}