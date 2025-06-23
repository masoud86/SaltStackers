using SaltStackers.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SaltStackers.Web.Helpers
{
    public class DynamicPermissionHandler : AuthorizationHandler<DynamicPermissionRequirement>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUtilities _utilities;
        private readonly IMemoryCache _memoryCache;
        private readonly IDataProtector _protectorToken;
        private readonly IMembershipService _membershipService;

        public DynamicPermissionHandler(IHttpContextAccessor contextAccessor, IUtilities utilities,
            IMemoryCache memoryCache, IDataProtectionProvider dataProtectionProvider,
            IMembershipService membershipService)
        {
            _contextAccessor = contextAccessor;
            _utilities = utilities;
            _memoryCache = memoryCache;
            _protectorToken = dataProtectionProvider.CreateProtector("RvgGuid");
            _membershipService = membershipService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DynamicPermissionRequirement requirement)
        {
            var httpContext = _contextAccessor.HttpContext;
            if (httpContext == null) return;

            var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return;

            var dbRoleValidationKey = _memoryCache.GetOrCreate("RoleValidationKey", p =>
            {
                p.AbsoluteExpiration = DateTimeOffset.MaxValue;
                return _utilities.DataBaseRoleValidationGuid();
            });

            SplitUserRequestedUrl(httpContext, out var areaAndActionAndControllerName);

            UnprotectRvgCookieData(httpContext, out var unprotectedRvgCookie);

            var user = await _membershipService.FindUserByIdAsync(userId);
            if (user == null) return;
            if (user.IsBlocked) return;

            if (!IsRvgCookieDataValid(unprotectedRvgCookie, userId, dbRoleValidationKey))
            {
                AddOrUpdateRvgCookie(httpContext, dbRoleValidationKey, userId);

                await _membershipService.RefreshSignInAsync(user);
            }

            if (httpContext.User.FindFirstValue(ClaimTypes.Role) == "Administrator" ||
                httpContext.User.HasClaim(areaAndActionAndControllerName, true.ToString()))
            {
                context.Succeed(requirement);
            }
        }

        private void SplitUserRequestedUrl(HttpContext httpContext, out string areaAndControllerAndActionName)
        {
            var areaName = httpContext.Request.RouteValues["area"]?.ToString() ?? "NoArea";
            var controllerName = httpContext.Request.RouteValues["controller"];
            var actionName = httpContext.Request.RouteValues["action"].ToString();
            areaAndControllerAndActionName = $"{areaName}|{controllerName}|{actionName}".ToUpper();
        }

        private void UnprotectRvgCookieData(HttpContext httpContext, out string unprotectedRvgCookie)
        {
            var protectedRvgCookie = httpContext.Request.Cookies
                .FirstOrDefault(t => t.Key == "RVG").Value;
            unprotectedRvgCookie = null;
            if (!string.IsNullOrEmpty(protectedRvgCookie))
            {
                try
                {
                    unprotectedRvgCookie = _protectorToken.Unprotect(protectedRvgCookie);
                }
                catch (CryptographicException)
                {
                }
            }
        }

        private bool IsRvgCookieDataValid(string rvgCookieData, string validUserId, string validRvg)
            => !string.IsNullOrEmpty(rvgCookieData) &&
               SplitUserIdFromRvgCookie(rvgCookieData) == validUserId &&
               SplitRvgFromRvgCookie(rvgCookieData) == validRvg;

        private string SplitUserIdFromRvgCookie(string rvgCookieData)
            => rvgCookieData.Split("|||")[1];

        private string SplitRvgFromRvgCookie(string rvgCookieData)
            => rvgCookieData.Split("|||")[0];

        private string CombineRvgWithUserId(string rvg, string userId)
            => rvg + "|||" + userId;

        private void AddOrUpdateRvgCookie(HttpContext httpContext, string validRvg, string validUserId)
        {
            var rvgWithUserId = CombineRvgWithUserId(validRvg, validUserId);
            var protectedRvgWithUserId = _protectorToken.Protect(rvgWithUserId);
            httpContext.Response.Cookies.Append("RVG", protectedRvgWithUserId,
                new CookieOptions
                {
                    MaxAge = TimeSpan.FromDays(90),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax
                });
        }
    }
}
