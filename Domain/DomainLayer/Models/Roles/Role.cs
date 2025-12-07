using System.Collections.Generic;
using DomainLayer.Models.Users;

namespace DomainLayer.Models.Roles
{
    public class Role:BaseEntity
    {
        public string RoleTitle { get; set; }

        #region Relations
        public List<UserRole> UserRoles { get; set; }
        public List<RolePermission> RolePermissions { get; set; }
        #endregion
    }
}