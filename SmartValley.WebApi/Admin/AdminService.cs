using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;

namespace SmartValley.WebApi.Admin
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;

        public AdminService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task AddAsync(Address address)
        {
            var user = await _userRepository.GetByAddressAsync(address);
            if (user == null)
                throw new AppErrorException(ErrorCode.UserNotFound);

            await _userRepository.AddRoleAsync(user.Id, RoleType.Admin);
            await _userRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(Address address)
        {
            var user = await _userRepository.GetByAddressAsync(address);
            if (user == null)
                throw new AppErrorException(ErrorCode.UserNotFound);

            await _userRepository.RemoveRoleAsync(user.Id, RoleType.Admin);
            await _userRepository.SaveChangesAsync();
        }

        public Task<IReadOnlyCollection<User>> GetAsync()
            => _userRepository.GetByRoleAsync(RoleType.Admin);
    }
}