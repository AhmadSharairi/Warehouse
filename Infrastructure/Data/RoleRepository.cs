using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore; 
using System.Threading.Tasks; 

namespace Infrastructure.Data
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Role> GetRoleByIdAsync(int roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }

        public async Task<int> GetRoleIdByNameAsync(string roleName)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            return role?.Id ?? 0; 
        }
    }
}
