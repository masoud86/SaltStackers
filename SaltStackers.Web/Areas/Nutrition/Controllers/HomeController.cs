using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SaltStackers.Web.Areas.Nutrition.Controllers
{
    [Area("Nutrition")]
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Title = "Nutrition", UseDefaultRouteUrl = true, RemoveAllDefaultRouteValues = false, Order = 0)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
