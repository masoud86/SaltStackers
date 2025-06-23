using System.Collections.Generic;

namespace SaltStackers.Application.ViewModels.Membership
{
    public class RolePermissions
    {
        public string RoleId { get; set; }

        public string RoleName { get; set; }

        public List<Permission> Permissions { get; set; }
    }
}
