using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Application.ViewModels.Membership;
using SaltStackers.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Customer
{
    public class CustomerAddresses : Pagination
    {
        public CustomerAddresses() : base("CreateDateTime")
        {
            Columns = new Dictionary<string, string> {
                {"CreateDateTime", Resources.Global.CreateTime}
            };
        }

        public List<CustomerAddressDto> Items { get; set; }
    }

    public class CustomerAddressFilters : Pagination
    {
        public CustomerAddressFilters() : base("CreateDateTime")
        {
        }

        public string CustomerId { get; set; }

        public bool ShowArchivedItems { get; set; }
    }

    public class CustomerAddressDto
    {
        public int? Id { get; set; }

        public int ZoneId { get; set; }

        //public ZoneDto Zone { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string? Unit { get; set; }

        public double? Longitude { get; set; }

        public double? Latitude { get; set; }

        public string? Instruction { get; set; }

        public Guid GroupId { get; set; }

        public bool IsArchived { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string UserId { get; set; }
        public UserDto? User { get; set; }
    }

    public class CustomerAddressApi
    {
        public int Id { get; set; }

        public int ZoneId { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Address")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        [StringLength(400, ErrorMessageResourceName = "StringLengthMax", ErrorMessageResourceType = typeof(Resources.Error))]
        [RegularExpression(PatternHelper.DangerousCharactersSimplify, ErrorMessageResourceName = "RegularExpression", ErrorMessageResourceType = typeof(Resources.Error))]
        public required string Address { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Postal Code")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        [StringLength(15, ErrorMessageResourceName = "StringLengthMax", ErrorMessageResourceType = typeof(Resources.Error))]
        [RegularExpression(PatternHelper.DangerousCharacters, ErrorMessageResourceName = "RegularExpression", ErrorMessageResourceType = typeof(Resources.Error))]
        public required string PostalCode { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Unit / Buzz")]
        [StringLength(50, ErrorMessageResourceName = "StringLengthMax", ErrorMessageResourceType = typeof(Resources.Error))]
        [RegularExpression(PatternHelper.DangerousCharacters, ErrorMessageResourceName = "RegularExpression", ErrorMessageResourceType = typeof(Resources.Error))]
        public string? Unit { get; set; }

        public double? Longitude { get; set; }

        public double? Latitude { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Instruction")]
        [StringLength(500, ErrorMessageResourceName = "StringLengthMax", ErrorMessageResourceType = typeof(Resources.Error))]
        [RegularExpression(PatternHelper.DangerousCharacters, ErrorMessageResourceName = "RegularExpression", ErrorMessageResourceType = typeof(Resources.Error))]
        public string? Instruction { get; set; }
    }
}
