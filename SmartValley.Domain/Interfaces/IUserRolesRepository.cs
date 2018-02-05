using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IUserRolesRepository
    {
        Task<IReadOnlyCollection<Role>> GetRolesByUserIdAsync(long userId);
    }
}