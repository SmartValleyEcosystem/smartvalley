using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Data.SQL.Repositories
{
    public class RolesRepository : IRoleRepository
    {
        private readonly IReadOnlyDataContext _readContext;

        public RolesRepository(IReadOnlyDataContext readContext)
        {
            _readContext = readContext;
        }

        public Task<Role> GetByNameAsync(RoleType type)
        {
            return _readContext.Roles.FirstOrDefaultAsync(i => i.Id == type);
        }
    }
}
