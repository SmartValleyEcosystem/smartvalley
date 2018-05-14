using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class UserRepository : EntityCrudRepository<User>, IUserRepository
    {
        public UserRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
            : base(readContext, editContext)
        {
        }

        public new async Task<IReadOnlyCollection<User>> GetAllAsync(int offset, int count)
        {
            return await ReadContext.Users
                             .Skip(offset)
                             .Take(count)
                             .ToArrayAsync();
        }

        public Task<int> GetTotalCountAsync()
            => ReadContext.Users.CountAsync();

        public Task<User> GetByAddressAsync(Address address)
            => ReadContext.Users.FirstOrDefaultAsync(u => u.Address == address);

        public async Task<IReadOnlyCollection<User>> GetByAddressesAsync(IReadOnlyCollection<Address> addresses)
        {
            return await ReadContext.Users
                                    .Where(user => addresses.Contains(user.Address))
                                    .ToArrayAsync();
        }

        public Task<User> GetByEmailAsync(string email)
            => ReadContext.Users.FirstOrDefaultAsync(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        public async Task AddRoleAsync(Address address, RoleType type)
        {
            var user = await ReadContext.Users.FirstOrDefaultAsync(i => i.Address == address);
            if (user == null)
                throw new AppErrorException(ErrorCode.UserNotFound, $"User at address '{address}' not found.");

            var role = await ReadContext.Roles.FirstOrDefaultAsync(i => i.Id == type);
            if (role == null)
                throw new AppErrorException(ErrorCode.RoleNotFound, $"Role '{type}' not found.");

            await EditContext.UserRoles.AddAsync(new UserRole {RoleId = role.Id, UserId = user.Id});
            await EditContext.SaveAsync();
        }

        public Task<bool> HasRoleAsync(Address address, RoleType type)
        {
            return (from userRole in ReadContext.UserRoles
                    join role in ReadContext.Roles on userRole.RoleId equals role.Id
                    join user in ReadContext.Users on userRole.UserId equals user.Id
                    where role.Id == type && user.Address == address
                    select user).AnyAsync();
        }

        public async Task RemoveRoleAsync(Address address, RoleType type)
        {
            var user = await ReadContext.Users.FirstOrDefaultAsync(i => i.Address == address);
            if (user == null)
                throw new AppErrorException(ErrorCode.UserNotFound, $"User at address '{address}' not found.");

            var role = await ReadContext.Roles.FirstOrDefaultAsync(i => i.Id == type);
            if (role == null)
                throw new AppErrorException(ErrorCode.RoleNotFound, $"Role '{type}' not found.");

            var userRole = await ReadContext.UserRoles.FirstOrDefaultAsync(i => i.RoleId == role.Id && i.UserId == user.Id);
            if (userRole == null)
                throw new AppErrorException(ErrorCode.UserHaveNoRole, $"User at address '{address}' has no '{type}' role.");

            EditContext.UserRoles.Remove(userRole);
            await EditContext.SaveAsync();
        }

        public async Task<IReadOnlyCollection<Role>> GetRolesByUserIdAsync(long userId)
        {
            return await (from userRole in ReadContext.UserRoles
                          join role in ReadContext.Roles on userRole.RoleId equals role.Id
                          join user in ReadContext.Users on userRole.UserId equals user.Id
                          where user.Id == userId
                          select role).ToArrayAsync();
        }

        public async Task<IReadOnlyCollection<User>> GetByRoleAsync(RoleType type)
        {
            return await (from userRole in ReadContext.UserRoles
                          join role in ReadContext.Roles on userRole.RoleId equals role.Id
                          join user in ReadContext.Users on userRole.UserId equals user.Id
                          where role.Id == type
                          select user).ToArrayAsync();
        }
    }
}