using SaltStackers.Application.ViewModels.Base;
using System.Collections.Generic;

namespace SaltStackers.Application.ViewModels.Log
{
    public class ApplicationLogs : Pagination
    {
        public ApplicationLogs() : base("LogDateTime")
        {
            Columns = new Dictionary<string, string> {
                {"LogDateTime", Resources.Global.CreateTime}
            };
        }

        public List<ApplicationLogDto> Items { get; set; }
    }

    public class ApplicationLogFilters : Pagination
    {
        public ApplicationLogFilters() : base("LogDateTime")
        {
        }

        public string UserId { get; set; }

        public string RequestNumber { get; set; }
    }
}
