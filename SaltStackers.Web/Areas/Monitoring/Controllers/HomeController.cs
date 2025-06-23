using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SaltStackers.Web.Areas.Monitoring.Controllers
{
    [Area("Monitoring")]
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Title = "Monitoring", UseDefaultRouteUrl = true, RemoveAllDefaultRouteValues = false, Order = 0)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
