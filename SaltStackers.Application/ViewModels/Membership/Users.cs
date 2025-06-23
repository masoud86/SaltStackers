using SaltStackers.Application.ViewModels.Base;

namespace SaltStackers.Application.ViewModels.Membership
{
    public class Users : Pagination
    {
        public Users() : base("CreateTime")
        {
            Columns = new Dictionary<string, string> {
                {"CreateTime", "Create Time"},
                {"Name", "Name"}
            };
        }

        public List<UserDto> Items { get; set; }

        //public Dictionary<string, string> Columns { get; set; }
    }

    public class UserFilters : Pagination
    {
        public UserFilters() : base("CreateTime")
        {
            
        }

        public string Role { get; set; }
    }
}
