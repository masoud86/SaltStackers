using SaltStackers.Application.ViewModels.Base;
using System.Collections.Generic;

namespace SaltStackers.Application.ViewModels.Membership
{
    public class Roles : Pagination
    {
        public Roles() : base("CreateTime")
        {
            Columns = new Dictionary<string, string> {
                {"CreateTime", "Recently Created"},
                {"Name", "Title"},
                {"DisplayName", "Display Name"}
            };
        }
        public List<RoleDto> Items { get; set; }
    }

    public class RoleFilters : Pagination
    {
        public RoleFilters() : base("CreateTime")
        {
            
        }
    }
}
