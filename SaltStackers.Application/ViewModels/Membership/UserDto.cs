using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Application.ViewModels.Operation.Kitchen;
using SaltStackers.Common.Enums;
using SaltStackers.Common.Helper;
using Humanizer;
using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Membership;

public class UserDto : UserLog
{
    public string Id { get; set; }

    [Display(Name = "Name")]
    public string? Name { get; set; }

    [Display(Name = "Username", ResourceType = typeof(Resources.Security))]
    public string Username { get; set; }

    [Display(Name = "Role", ResourceType = typeof(Resources.Security))]
    public string Role { get; set; }

    [Display(Name = "PhoneNumber", ResourceType = typeof(Resources.Global))]
    public string PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    [Display(Name = "Email", ResourceType = typeof(Resources.Global))]
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }

    public bool IsBlocked { get; set; }

    [Display(Name = "RegisterTime", ResourceType = typeof(Resources.Global))]
    public DateTime CreateDateTime { get; set; }
    public DateTime CreateDateTimeLocal => CreateDateTime.ConvertFromUtc();
    public string CreateDateTimeHumanized => DateTime.UtcNow.Add(-(DateTime.UtcNow - CreateDateTime)).Humanize();

    public string? StripeId { get; set; }

    public DateTime? BirthDate { get; set; }
    public string BirthDateFormatted => BirthDate.HasValue ? BirthDate.Value.ToShortDateString() : "";

    public Gender? Gender { get; set; }
    public string GenderTitle => Gender.HasValue ? EnumHelper<Gender>.GetDisplayValue(Gender.Value) : "";

    public BloodType? BloodType { get; set; }
    public string? BloodTypeTitle => BloodType.HasValue ? EnumHelper<BloodType>.GetDisplayValue(BloodType.Value) : "";

    public bool CoolingBag { get; set; }

    public double CoolingBagDeposit { get; set; }


    public DateTime EditDateTime { get; set; }
    public string EditDateTimeLocal => EditDateTime.ConvertFromUtcString();

    [Display(Name = "RegisterTime", ResourceType = typeof(Resources.Global))]
    public string CreateDate => CreateDateTime != DateTime.MinValue ? CreateDateTime.ToShortDateString() : "";

    public bool IsAdmin { get; set; }

    public string? Referral { get; set; }

    public bool IsPartner { get; set; }

    public int? KitchenId { get; set; }

    public KitchenDto? Kitchen { get; set; }

    public DateTime? LastLogin { get; set; }
    public DateTime? LastLoginLocal => LastLogin.HasValue ? LastLogin.Value.ConvertFromUtc() : null;
    public string LastLoginHumanized => LastLogin.HasValue
        ? DateTime.UtcNow.Add(-(DateTime.UtcNow - LastLogin.Value)).Humanize()
        : "";

    public RoleDto? RoleModel { get; set; }

    public string? About { get; set; }
}
