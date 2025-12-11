using System.Collections.Generic;
using System.Threading.Tasks;
using DomainLayer.Enums;
using DomainLayer.Models.Roles;

namespace CoreLayer.Services.Users
{
    public interface IUserRoleService
    {
        Task<List<Role>> GetAllRole();
        Task EditUserRoles(List<long> roles,long userId);
        Task<bool> AddRoleForUser(long roleId, long userId);
        Task AddRole(Role role, List<Permissions> permissions);
        Task EditRole(Role role, List<Permissions> permissions);
        Task<Role> GetRoleById(long roleId);
        Task<Role> GetRoleNoTask(long roleId);
        Task<bool> DeleteRole(long roleId);
    }
}