using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class UserRolesRepository : IUserRolesRepository
    {
        private readonly IReadOnlyDataContext _readContext;

        public UserRolesRepository(IReadOnlyDataContext readContext)
        {
            _readContext = readContext;
        }

        public async Task<IReadOnlyCollection<Role>> GetRolesByUserIdAsync(long userId)
        {
            return await (from userRole in _readContext.UserRoles
                          join role in _readContext.Roles on userRole.RoleId equals role.Id
                          where userRole.UserId == userId
                          select role).ToArrayAsync();
        }
    }
}