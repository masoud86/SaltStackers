using System.ComponentModel.DataAnnotations.Schema;

namespace SaltStackers.Domain.Models.Log
{
    public class UserActivityLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public string DescriptionResourceKey { get; set; }

        public string DescriptionParameters { get; set; }

        public string Type { get; set; }

        public string ActionRelatedId { get; set; }

        public string Content { get; set; }

        public string IpAddress { get; set; }

        public string Device { get; set; }
        
        public string Browser { get; set; }

        public string BrowserVersion { get; set; }

        public string OperatingSystem { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
