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
    public class UserRepository : IUserRepository
    {
        private readonly IReadOnlyDataContext _readContext;
        private readonly IEditableDataContext _editContext;

        public UserRepository(IReadOnlyDataContext readContext, IEditableDataContext editContext)
        {
            _readContext = readContext;
            _editContext = editContext;
        }

        public void Add(User user)
        {
            _editContext.Users.Add(user);
        }

        public Task<User> GetByAddressAsync(Address address)
            => _editContext.Users.FirstOrDefaultAsync(u => u.Address == address);
        
        public Task<User> GetByEmailAsync(string email)
            => _editContext.Users.FirstOrDefaultAsync(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        public Task<User> GetByIdAsync(long userId) 
            => _editContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

        public async Task AddRoleAsync(Address address, RoleType type)
        {
            var user = await _readContext.Users.FirstOrDefaultAsync(i => i.Address == address);
            if (user == null)
                throw new AppErrorException(ErrorCode.UserNotFound, $"User at address '{address}' not found.");

            var role = await _readContext.Roles.FirstOrDefaultAsync(i => i.Id == type);
            if (role == null)
                throw new AppErrorException(ErrorCode.RoleNotFound, $"Role '{type}' not found.");

            await _editContext.UserRoles.AddAsync(new UserRole {RoleId = role.Id, UserId = user.Id});
            await _editContext.SaveAsync();
        }

        public Task<bool> HasRoleAsync(Address address, RoleType type)
        {
            return (from userRole in _readContext.UserRoles
                    join role in _readContext.Roles on userRole.RoleId equals role.Id
                    join user in _readContext.Users on userRole.UserId equals user.Id
                    where role.Id == type && user.Address == address
                    select user).AnyAsync();
        }

        public async Task RemoveRoleAsync(Address address, RoleType type)
        {
            var user = await _readContext.Users.FirstOrDefaultAsync(i => i.Address == address);
            if (user == null)
                throw new AppErrorException(ErrorCode.UserNotFound, $"User at address '{address}' not found.");

            var role = await _readContext.Roles.FirstOrDefaultAsync(i => i.Id == type);
            if (role == null)
                throw new AppErrorException(ErrorCode.RoleNotFound, $"Role '{type}' not found.");

            var userRole = await _readContext.UserRoles.FirstOrDefaultAsync(i => i.RoleId == role.Id && i.UserId == user.Id);
            if (userRole == null)
                throw new AppErrorException(ErrorCode.UserHaveNoRole, $"User at address '{address}' has no '{type}' role.");

            _editContext.UserRoles.Remove(userRole);
            await _editContext.SaveAsync();
        }

        public async Task<IReadOnlyCollection<Role>> GetRolesByUserIdAsync(long userId)
        {
            return await (from userRole in _readContext.UserRoles
                          join role in _readContext.Roles on userRole.RoleId equals role.Id
                          join user in _readContext.Users on userRole.UserId equals user.Id
                          where user.Id == userId
                          select role).ToArrayAsync();
        }
        
        public async Task SaveChangesAsync()
        {
            await _editContext.SaveAsync();
        }

        public async Task<IReadOnlyCollection<User>> GetAllAsync(int offset, int count)
        {
            return await _readContext.Users
                                    .Skip(offset)
                                    .Take(count)
                                    .ToArrayAsync();
        }

        public async Task<IReadOnlyCollection<User>> GetByAddressesAsync(IReadOnlyCollection<Address> addresses)
        {
            return await _readContext.Users
                                    .Where(user => addresses.Contains(user.Address))
                                    .ToArrayAsync();
        }

        public async Task<IReadOnlyCollection<User>> GetByRoleAsync(RoleType type)
        {
            return await (from userRole in _readContext.UserRoles
                          join role in _readContext.Roles on userRole.RoleId equals role.Id
                          join user in _readContext.Users on userRole.UserId equals user.Id
                          where role.Id == type
                          select user).ToArrayAsync();
        }

        public Task<int> GetTotalCountAsync()
            => _readContext.Users.CountAsync();
    }
}