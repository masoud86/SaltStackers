using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SaltStackers.Web.Areas.Financial.Controllers
{
    [Area("Financial")]
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Title = "Financial", UseDefaultRouteUrl = true, RemoveAllDefaultRouteValues = false, Order = 0)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
