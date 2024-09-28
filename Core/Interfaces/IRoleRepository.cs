
using Core.Entities;

namespace Core.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> GetRoleByIdAsync(int roleId);
        Task<int> GetRoleIdByNameAsync(string roleName);

    
    }
}
