using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Log;
using SaltStackers.Application.ViewModels.Membership;
using SaltStackers.Common.Helper;
using SaltStackers.Web.Helpers;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SaltStackers.Web.Areas.Membership.Controllers
{
    [Area("Membership")]
    [BreadCrumb(Title = "Membership", Order = 0, Url = "/Membership")]
    public class UserController : Controller
    {
        private readonly IMembershipService _membershipService;
        private readonly ILogService _logService;

        public UserController(IMembershipService membershipService, ILogService logService)
        {
            _membershipService = membershipService;
            _logService = logService;
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 1, Title = "Users", Url = "/Membership/User")]
        public async Task<IActionResult> Index(UserFilters model)
        {
            var users = await _membershipService.GetUsersAsync(model);
            return View(users);
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Users", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> Create()
        {
            var model = new CreateUser
            {
                Roles = await _membershipService.GetSelectableRolesAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        [Log]
        public async Task<IActionResult> Create(CreateUser model)
        {
            if (ModelState.IsValid)
            {
                var createUser = await _membershipService.CreateUserAsync(model);

                if (createUser.Succeeded)
                {
                    var addUserToRole = await _membershipService.AddUserToRoleAsync(model.PhoneNumber, model.Role);

                    if (addUserToRole.Succeeded)
                    {
                        //TODO: Send SMS

                        return RedirectToAction("Index", "Home");
                    }

                    foreach (var error in addUserToRole.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                foreach (var error in createUser.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            model.Roles = await _membershipService.GetSelectableRolesAsync();

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Users", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _membershipService.GetUserForEditAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Roles = await _membershipService.GetSelectableRolesAsync();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        public async Task<IActionResult> Edit(EditUser model)
        {
            if (ModelState.IsValid)
            {
                var result = await _membershipService.UpdateUserAsync(model);
                if (result.Succeeded)
                {
                    var addUserToRole = await _membershipService.AddUserToRoleAsync(model.PhoneNumber, model.Role);
                    if (addUserToRole.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                    foreach (var error in addUserToRole.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            model.Roles = await _membershipService.GetSelectableRolesAsync();

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Users", UseDefaultRouteUrl = true)]
        public IActionResult ChangePassword()
        {
            return View(new ChangePassword());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                model.Id = User.Id();
                var result = await _membershipService.ChangePasswordAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Profile");
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
        [BreadCrumb(Order = 2, Title = "Users", UseDefaultRouteUrl = true)]
        public IActionResult ResetPassword(string id)
        {
            if (id == User.Id())
            {
                return NotFound();
            }

            return View(new ResetPassword { Id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            if (model.Id == User.Id())
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _membershipService.ResetPasswordAsync(model);
                if (result.Succeeded)
                {
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
        [BreadCrumb(Order = 2, Title = "Users", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> Profile(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                id = User.Id();
            }

            var userInformation = await _membershipService.GetUserInformationAsync(id);

            if (userInformation == null)
            {
                return NotFound();
            }

            return View(new UserDto { Id = id, Name = userInformation.Name });
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Users", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> Block(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _membershipService.GetUserInformationAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        public async Task<IActionResult> Block(UserDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _membershipService.ChangeUserAccessAsync(model, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Users", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> Unblock(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _membershipService.GetUserInformationAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        public async Task<IActionResult> UnBlock(UserDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _membershipService.ChangeUserAccessAsync(model, true);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsEmailFree(string email)
        {
            var isEmailFree = await _membershipService.IsEmailFree(email);
            if (isEmailFree)
            {
                return Json(true);
            }

            return Json(string.Format(Resources.Error.DuplicateEmail, email));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsUsernameFree(string username)
        {
            var isUsernameFree = await _membershipService.IsUsernameFree(username);
            if (isUsernameFree)
            {
                return Json(true);
            }

            return Json(string.Format(Resources.Error.DuplicateUserName, username));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetUserLogDetailsById(Guid id)
        {
            var log = await _logService.GetUserActivityLogById(id);
            return Json(log);
        }
    }
}
