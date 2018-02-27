using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public Task<User> GetByAddressAsync(string address)
            => _repository.GetByAddressAsync(address);

        [HttpPut]
        public async Task UpdateAsync(string address, string name, string about)
        {
            var user = await _repository.GetByAddressAsync(address);

            user.Name = name;
            user.About = about;

            await _repository.UpdateWholeAsync(user);
        }
    }
}