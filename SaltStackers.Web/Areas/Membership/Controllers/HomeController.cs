using SaltStackers.Application.Interfaces;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SaltStackers.Web.Areas.Membership.Controllers
{
    [Area("Membership")]
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Title = "Membership", UseDefaultRouteUrl = true, RemoveAllDefaultRouteValues = false, Order = 0)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
