using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Customer;
using SaltStackers.Web.Helpers;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SaltStackers.Web.Areas.Membership.Controllers
{
    [Area("Membership")]
    [BreadCrumb(Title = "Membership", Order = 0, Url = "/Membership")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 1, Title = "Users", Url = "/Membership/User")]
        public async Task<IActionResult> Index(CustomerFilters model)
        {
            return View(await _customerService.GetCustomersModelAsync(model));
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Users", UseDefaultRouteUrl = true)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        [Log]
        public async Task<IActionResult> Create(CustomerDto model)
        {
            if (ModelState.IsValid)
            {
                var createCustomer = await _customerService.CreateCustomerAsync(model);

                if (createCustomer.Succeeded)
                {
                    return RedirectToAction("Index", "Customer", new { Area = "Membership" });
                }

                foreach (var error in createCustomer.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

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

            var user = await _customerService.GetCustomerAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        public async Task<IActionResult> Edit(CustomerDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _customerService.UpdateCustomerAsync(model);
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
    }
}
