using SaltStackers.Application.Helpers;
using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Membership;
using SaltStackers.Common.Helper;
using SaltStackers.Web.Helpers;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace SaltStackers.Web.Areas.Membership.Controllers
{
    [Area("Membership")]
    [BreadCrumb(Title = "Membership", Order = 0, Url = "/Membership")]
    public class RoleController : Controller
    {
        private readonly IMembershipService _membershipService;
        private readonly ILogService _logService;
        private readonly IMemoryCache _memoryCache;
        private readonly IUtilities _utilities;

        public RoleController(IMembershipService membershipService, ILogService logService,
            IMemoryCache memoryCache, IUtilities utilities)
        {
            _membershipService = membershipService;
            _logService = logService;
            _memoryCache = memoryCache;
            _utilities = utilities;
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 1, Title = "Roles", Url = "/Membership/Role")]
        public async Task<IActionResult> Index(RoleFilters model)
        {
            var roles = await _membershipService.GetRolesAsync(model);
            return View(roles);
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Roles", UseDefaultRouteUrl = true)]
        public IActionResult Create()
        {
            var model = new CreateRole();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        public async Task<IActionResult> Create(CreateRole model)
        {
            if (ModelState.IsValid)
            {
                model.OwnerId = User.Id();
                var result = await _membershipService.CreateRoleAsync(model);

                if (result.Succeeded)
                {
                    var createdRole = await _membershipService.FindRoleByNameAsync(model.Name);
                    await _logService.AddUserLogAsync(User.Id(), "Role", "CreateNewRole", model.Name, createdRole.Id, model, Request.GetClientInfo());
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Roles", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var role = await _membershipService.GetRoleForDeleteAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        public async Task<IActionResult> Delete(DeleteRole model)
        {
            if (ModelState.IsValid)
            {
                var result = await _membershipService.DeleteRoleAsync(model);
                if (result.Succeeded)
                {
                    await _logService.AddUserLogAsync(User.Id(), "Role", "DeleteRole", model.Name, model, Request.GetClientInfo());
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Roles", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var role = await _membershipService.GetRoleForEditAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        public async Task<IActionResult> Edit(EditRole model)
        {
            if (ModelState.IsValid)
            {
                var result = await _membershipService.UpdateRoleAsync(model);
                if (result.Succeeded)
                {
                    await _logService.AddUserLogAsync(User.Id(), "Role", "EditRole", model.Name, model.Id, model, Request.GetClientInfo());
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Roles", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> Permissions(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var role = await _membershipService.FindRoleByIdAsync(id);

            if (role == null || role.IsLocked)
            {
                return NotFound();
            }

            this.SetCurrentBreadCrumbTitle(string.Format(Resources.Security.RolePermissions, role.Name));

            var applicationPermissions =
                _memoryCache.GetOrCreate("ApplicationPermissions", p =>
                {
                    p.AbsoluteExpiration = DateTimeOffset.MaxValue;
                    return _utilities.GetApplicationPages();
                });

            var permissions = _utilities.GetPermissionsByMethods(applicationPermissions);

            return View(new RolePermissions
            {
                RoleId = role.Id,
                RoleName = role.Name,
                Permissions = await _membershipService.CurrentClaimsAsync(role.Id, permissions)
            });
        }

        [HttpPost]
        [BreadCrumb(Order = 1)]
        [Authorize(Policy = "DynamicPermission")]
        public async Task<IActionResult> Permissions(RolePermissions model)
        {
            this.SetCurrentBreadCrumbTitle(string.Format(Resources.Security.RolePermissions, model.RoleName));
            if (ModelState.IsValid)
            {
                await _membershipService.ManageRoleClaimsAsync(model.RoleId, model.Permissions);
            }

            return View(model);
        }
    }
}
