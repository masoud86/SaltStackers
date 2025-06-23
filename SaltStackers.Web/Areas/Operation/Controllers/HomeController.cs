using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SaltStackers.Web.Areas.Operation.Controllers
{
    [Area("Operation")]
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Title = "Operation", UseDefaultRouteUrl = true, RemoveAllDefaultRouteValues = false, Order = 0)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
