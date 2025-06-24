using SaltStackers.Common.Helper;
using System;

namespace SaltStackers.Application.ViewModels.Log
{
    public class ApplicationLogDto
    {
        public Guid Id { get; set; }

        public string Level { get; set; }

        public string Message { get; set; }

        public string Logger { get; set; }

        public string Parameters { get; set; }

        public string ReceiptNumber { get; set; }

        public string RequestNumber { get; set; }

        public string GroupKey { get; set; }

        public string UserId { get; set; }

        public DateTime LogDateTime { get; set; }
        public string LogDateTimeLocal => LogDateTime.ConvertFromUtcString();
    }
}
