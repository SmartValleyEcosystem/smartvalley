using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Admin
{
    public class AdminService : IAdminService
    {
        public Task AddAsync(string address)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string address)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsAdminAsync(string address)
        {
            throw new NotImplementedException();
        }
    }
}
