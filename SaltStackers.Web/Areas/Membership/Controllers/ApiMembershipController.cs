using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Application.ViewModels.Customer;
using SaltStackers.Application.ViewModels.Membership;
using SaltStackers.Application.ViewModels.Membership.User;
using SaltStackers.Web.Helpers;

namespace SaltStackers.Web.Areas.Membership.Controllers
{
    [ApiController]
    public class ApiMembershipController : ControllerBase
    {
        private readonly IMembershipService _membershipService;
        private readonly ICustomerService _customerService;
        private readonly IUploadService _uploadService;

        public ApiMembershipController(IMembershipService membershipService, ICustomerService customerService, IUploadService uploadService)
        {
            _membershipService = membershipService;
            _customerService = customerService;
            _uploadService = uploadService;
        }

        private static List<ServiceError> ModelStateToServiceError(ModelStateDictionary modelstate)
        {
            var errors = new List<ServiceError>();
            foreach (var error in modelstate.Values.SelectMany(v => v.Errors))
            {
                errors.Add(new ServiceError
                {
                    Level = ErrorLevel.Blocker,
                    Description = error.ErrorMessage
                });
            }
            return errors;
        }
        
        [Log]
        [Authorize]
        [HttpGet("Api/Membership/GetUserProfile")]
        public async Task<IActionResult> GetUserProfile(string id)
        {
            return Ok(await _membershipService.GetUserInformationAsync(id));
        }

        [Log]
        [Authorize]
        [HttpPost("Api/Membership/EditUsername")]
        public async Task<IActionResult> EditUsername(EditUsername model)
        {
            var errors = new List<ServiceError>();

            if (ModelState.IsValid)
            {
                var result = await _customerService.EditUsernameAsync(model);

                if (result.Succeeded)
                {
                    return Ok(new ServiceResult(true));
                }

                foreach (var error in result.Errors)
                {
                    errors.Add(new ServiceError
                    {
                        Level = ErrorLevel.Blocker,
                        Description = error.Description
                    });
                }

                return Ok(new ServiceResult(false, errors));
            }

            errors.AddRange(ModelStateToServiceError(ModelState));

            return BadRequest(errors.FirstOrDefault()?.Description);
        }

        [Log]
        [Authorize]
        [HttpPost("Api/Membership/EditAbout")]
        public async Task<IActionResult> EditAbout(EditAbout model)
        {
            var errors = new List<ServiceError>();

            if (ModelState.IsValid)
            {
                var result = await _customerService.EditAboutAsync(model);

                if (result.Succeeded)
                {
                    return Ok(new ServiceResult(true));
                }

                foreach (var error in result.Errors)
                {
                    errors.Add(new ServiceError
                    {
                        Level = ErrorLevel.Blocker,
                        Description = error.Description
                    });
                }

                return Ok(new ServiceResult(false, errors));
            }

            errors.AddRange(ModelStateToServiceError(ModelState));

            return BadRequest(errors.FirstOrDefault()?.Description);
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Customer/Search")]
        public async Task<IActionResult> Search(string query)
        {
            var model = await _customerService.GetCustomersAsync(new CustomerFilters
            {
                Query = query
            });
            return Ok(model);
        }

        [Authorize]
        [HttpGet("Api/Membership/GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await _membershipService.GetRolesAsync(new RoleFilters { PageSize = 100, Sort = "Name", Direction = "asc" }));
        }

        [Log]
        [Authorize]
        [HttpPost("Api/Membership/SwitchRole")]
        public async Task<IActionResult> SwitchRole(SwitchRole model)
        {
            var errors = new List<ServiceError>();

            if (ModelState.IsValid)
            {
                var result = await _membershipService.SwitchRoleAsync(model);

                if (result.Succeeded)
                {
                    return Ok(new ServiceResult(true));
                }

                foreach (var error in result.Errors)
                {
                    errors.Add(new ServiceError
                    {
                        Level = ErrorLevel.Blocker,
                        Description = error.Description
                    });
                }

                return Ok(new ServiceResult(false, errors));
            }

            errors.AddRange(ModelStateToServiceError(ModelState));

            return BadRequest(errors.FirstOrDefault()?.Description);
        }
    }
}
