using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Data.SQL.Extensions;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    // ReSharper disable once ClassNeverInstantiated.Global
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

        public async Task AddRoleAsync(long userId, RoleType type)
        {
            var role = await _readContext.Roles.FirstOrDefaultAsync(i => i.Id == type);
            if (role == null)
                throw new AppErrorException(ErrorCode.RoleNotFound, $"Role '{type}' not found.");

            _editContext.UserRoles.Add(new UserRole {RoleId = role.Id, UserId = userId});
        }

        public Task<bool> HasRoleAsync(Address address, RoleType type)
        {
            return (from userRole in _readContext.UserRoles
                    join role in _readContext.Roles on userRole.RoleId equals role.Id
                    join user in _readContext.Users on userRole.UserId equals user.Id
                    where role.Id == type && user.Address == address
                    select user).AnyAsync();
        }

        public async Task RemoveRoleAsync(long userId, RoleType type)
        {
            var userRole = await _readContext.UserRoles.FirstOrDefaultAsync(i => i.RoleId == type && i.UserId == userId);
            if (userRole == null)
                throw new AppErrorException(ErrorCode.UserHaveNoRole, $"User with Id = '{userId}' has no '{type}' role.");

            _editContext.UserRoles.Remove(userRole);
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

        public Task<PagingCollection<User>> GetAsync(int offset, int count)
            => _editContext.Users.GetPageAsync(offset, count);

        public async Task<IReadOnlyCollection<User>> GetByAddressesAsync(IReadOnlyCollection<Address> addresses)
        {
            return await _editContext.Users
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
    }
}