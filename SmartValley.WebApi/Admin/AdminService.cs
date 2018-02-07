using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.WebApi.Admin
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;

        public AdminService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task AddAsync(string address)
            => _userRepository.AddRoleAsync(address, RoleType.Admin);

        public Task DeleteAsync(string address)
            => _userRepository.RemoveRoleAsync(address, RoleType.Admin);

        public Task<IReadOnlyCollection<User>> GetAllAsync()
            => _userRepository.GetByRoleAsync(RoleType.Admin);

        public Task<bool> IsAdminAsync(string address)
            => _userRepository.HasRoleAsync(address, RoleType.Admin);
    }
}
