using SaltStackers.Application.ViewModels.Base;
using System.Collections.Generic;

namespace SaltStackers.Application.ViewModels.Log
{
    public class UserActivityLogs : Pagination
    {
        public UserActivityLogs() : base("CreateDateTime")
        {
            Columns = new Dictionary<string, string> {
                {"CreateDateTime", Resources.Global.CreateTime}
            };
        }

        public List<UserActivityLogDto> Items { get; set; }
    }

    public class UserActivityLogFilters : Pagination
    {
        public UserActivityLogFilters() : base("CreateDateTime")
        {
        }

        public string UserId { get; set; }
    }
}
