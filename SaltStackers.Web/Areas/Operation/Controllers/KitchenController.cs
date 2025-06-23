using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Operation.Kitchen;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SaltStackers.Web.Areas.Operation.Controllers
{
    [Area("Operation")]
    [BreadCrumb(Title = "Operation", Order = 0, Url = "/Operation")]
    public class KitchenController : Controller
    {
        private readonly IOperationService _operationService;

        public KitchenController(IOperationService operationService)
        {
            _operationService = operationService;
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 1, Title = "Kitchens", Url = "/Operation/Kitchen")]
        public async Task<IActionResult> Index(KitchenFilters model)
        {
            model.ShowAll = true;
            return View(await _operationService.GetKitchensModelAsync(model));
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 2, Title = "Details", UseDefaultRouteUrl = true)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return View(await _operationService.GetKitchenAsync(id.Value));
        }
    }
}
