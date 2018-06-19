using System.Linq;
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
        private readonly IUserRepository _userRepository;
        private readonly IExpertRepository _expertRepository;
        private readonly IExpertsRegistryContractClient _expertsRegistryContractClient;

        public ExpertService(
            IUserRepository userRepository,
            IExpertRepository expertRepository,
            IExpertsRegistryContractClient expertsRegistryContractClient)
        {
            _userRepository = userRepository;
            _expertRepository = expertRepository;
            _expertsRegistryContractClient = expertsRegistryContractClient;
        }

        public async Task<Expert> GetByAddressAsync(string address)
            => await _expertRepository.GetByAddressAsync(address) ?? throw new AppErrorException(ErrorCode.ExpertNotFound);

        public async Task UpdateExpertAreasAsync(long expertId)
        {
            var expert = await _expertRepository.GetByIdAsync(expertId) ?? throw new AppErrorException(ErrorCode.ExpertNotFound);
            var areas = await _expertsRegistryContractClient.GetExpertAreasAsync(expert.User.Address);

            if (!areas.Any())
            {
                await RemoveExpertAsync(expert);
            }
            else
            {
                expert.SetAreas(areas);
                await _expertRepository.SaveChangesAsync();
            }
        }

        private async Task RemoveExpertAsync(Expert expert)
        {
            await _userRepository.RemoveRoleAsync(expert.UserId, RoleType.Expert);
            await _userRepository.SaveChangesAsync();

            _expertRepository.Remove(expert);
            await _expertRepository.SaveChangesAsync();
        }
    }
}