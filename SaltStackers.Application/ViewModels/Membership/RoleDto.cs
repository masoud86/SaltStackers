using SaltStackers.Common.Helper;

namespace SaltStackers.Application.ViewModels.Membership
{
    public class RoleDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool IsLocked { get; set; }

        public string? Icon { get; set; }

        public DateTime CreateDateTime { get; set; }
        public string CreateDateTimeLocal => CreateDateTime.ConvertFromUtcString();
    }
}
