using DataLayer.Context;
using DomainLayer.Models.Roles;
using DomainLayer.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace CoreLayer.Services.Users
{
    public class UserRoleService : BaseService, IUserRoleService
    {
        public UserRoleService(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Role>> GetAllRole()
        {
            return await Table<Role>().ToListAsync();
        }

        public async Task EditUserRoles(List<long> roles, long userId)
        {
            var userRoles = Table<UserRole>().Where(r => r.UserId == userId);
            foreach (var role in userRoles)
            {
                Delete(role);
            }
            foreach (var item in roles)
            {
                Insert(new UserRole()
                {
                    RoleId = item,
                    UserId = userId
                });
            }
            await Save();
        }

        public async Task<bool> AddRoleForUser(long roleId, long userId)
        {
            try
            {
                Insert(new UserRole()
                {
                    RoleId = roleId,
                    UserId = userId
                });
                await Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task AddRole(Role role, List<Permissions> permissions)
        {
            var rolePermissions = new List<RolePermission>();
            foreach (var permission in permissions)
            {
                rolePermissions.Add(new RolePermission()
                {
                    PermissionId = permission
                });
            }

            role.RolePermissions = rolePermissions;
            Insert(role);
            await Save();
        }

        public async Task EditRole(Role role, List<Permissions> permissions)
        {
            var rolePermissions = new List<RolePermission>();
            var oldPermissions = _context.RolePermissions.Where(p => p.RoleId == role.Id);
            foreach (var permission in permissions)
            {
                rolePermissions.Add(new RolePermission()
                {
                    PermissionId = permission,
                    RoleId = role.Id
                });
            }

            if (oldPermissions.Any())
            {
                Delete(oldPermissions);
            }
            if (rolePermissions.Any())
            {
                Insert(rolePermissions);
            }
            Update(role);
            await Save();
        }
        public async Task<Role> GetRoleById(long roleId)
        {
            return await GetById<Role>(roleId, "RolePermissions");
        }

        public async Task<Role> GetRoleNoTask(long roleId)
        {
            return await Table<Role>().AsNoTracking()
                .Include(c => c.RolePermissions).SingleOrDefaultAsync(r => r.Id == roleId);
        }

        public async Task<bool> DeleteRole(long roleId)
        {
            var role = await GetById<Role>(roleId, new[] { "UserRoles" });
            if (role.RoleTitle == "فروشنده"  || role.RoleTitle == "کاربر" || role.RoleTitle == "ادمین")
                return false;

            if (role.UserRoles.Any()) return false;

            Delete(role);
            await Save();
            return true;
        }
    }
}