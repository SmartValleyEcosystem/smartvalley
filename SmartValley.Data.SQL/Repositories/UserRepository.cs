using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class UserRepository : EntityCrudRepository<User>, IUserRepository
    {
        public UserRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
            : base(readContext, editContext)
        {
        }

        public Task<User> GetByAddressAsync(string address)
        {
            return ReadContext.Users.FirstOrDefaultAsync(u => u.Address.Equals(address, StringComparison.OrdinalIgnoreCase));
        }

        public Task<User> GetByEmailAsync(string email)
        {
            return ReadContext.Users.FirstOrDefaultAsync(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
    }
}