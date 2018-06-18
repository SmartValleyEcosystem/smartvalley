using System.Threading.Tasks;
using SmartValley.Domain.Contracts;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Domain.Services
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ExpertService : IExpertService
    {
        private readonly IExpertRepository _expertRepository;
        private readonly IExpertsRegistryContractClient _expertsRegistryContractClient;

        public ExpertService(IExpertRepository expertRepository, IExpertsRegistryContractClient expertsRegistryContractClient)
        {
            _expertRepository = expertRepository;
            _expertsRegistryContractClient = expertsRegistryContractClient;
        }

        public async Task<Expert> GetByAddressAsync(string address)
            => await _expertRepository.GetByAddressAsync(address) ?? throw new AppErrorException(ErrorCode.ExpertNotFound);

        public async Task UpdateExpertAreasAsync(long expertId)
        {
            var expert = await _expertRepository.GetByIdAsync(expertId) ?? throw new AppErrorException(ErrorCode.ExpertNotFound);
            var areas = await _expertsRegistryContractClient.GetExpertAreasAsync(expert.User.Address);

            expert.SetAreas(areas);

            await _expertRepository.SaveChangesAsync();
        }
    }
}