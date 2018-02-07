using System.Threading.Tasks;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> GetByNameAsync(RoleType type);
    }
}
