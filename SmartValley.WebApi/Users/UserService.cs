using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.WebApi.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public Task<IReadOnlyCollection<User>> GetAllAsync(int offset, int count) => _repository.GetAllAsync(offset, count);

        public async Task SetCanCreatePrivateProjectsAsync(Address address, bool canCreatePrivateProjects)
        {
            var user = await _repository.GetByAddressAsync(address);

            user.CanCreatePrivateProjects = canCreatePrivateProjects;

            await _repository.SaveChangesAsync();
        }

        public Task<User> GetByAddressAsync(Address address)
            => _repository.GetByAddressAsync(address);

        public Task<User> GetByIdAsync(long id)
            => _repository.GetByIdAsync(id);
        
        public Task<int> GetTotalCountAsync() => _repository.GetTotalCountAsync();

        public async Task UpdateAsync(Address address, string firstName, string secondName)
        {
            var user = await _repository.GetByAddressAsync(address);

            user.FirstName = firstName;
            user.SecondName = secondName;

            await _repository.SaveChangesAsync();
        }
    }
}