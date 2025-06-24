using SaltStackers.Common.Helper;
using System;

namespace SaltStackers.Application.ViewModels.Log
{
    public class UserActivityLogDto : ClientInformation
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public string DescriptionResourceKey { get; set; }

        public string Description { get; set; }

        public string DescriptionParameters { get; set; }

        public string Type { get; set; }

        public string ActionRelatedId { get; set; }

        public object Content { get; set; }

        public string ContentString { get; set; }

        public string ReceiptNumber { get; set; }

        public string RequestNumber { get; set; }

        public DateTime CreateDateTime { get; set; }
        public string CreateDateTimeLocal => CreateDateTime.ConvertFromUtcString();
    }
}
