using SaltStackers.Domain.Models.Membership;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace SaltStackers.Application.Custom
{
    public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<AspNetUser>
    {
        private readonly UserManager<AspNetUser> _userManager;
        private readonly RoleManager<AspNetRole> _roleManager;

        public CustomUserClaimsPrincipalFactory(UserManager<AspNetUser> userManager,
            RoleManager<AspNetRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, optionsAccessor)
        {
            _userManager =  userManager;
            _roleManager = roleManager;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AspNetUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var currentRole = roles.FirstOrDefault();
            var role = await _roleManager.FindByNameAsync(currentRole);

            var principal = await base.GenerateClaimsAsync(user);
            principal.AddClaim(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? ""));
            principal.AddClaim(new Claim(ClaimTypes.Name, user.Name ?? ""));
            principal.AddClaim(new Claim(ClaimTypes.Role, currentRole ?? ""));
            principal.AddClaim(new Claim("RoleDisplayName", role.DisplayName ?? ""));

            var roleClaims = await _roleManager.GetClaimsAsync(role);
            principal.AddClaims(roleClaims);

            return principal;
        }
    }
}
