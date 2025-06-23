using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Settings;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SaltStackers.Web.Areas.Configuration.Controllers
{
    [Area("Configuration")]
    [BreadCrumb(TitleResourceName = "Settings", TitleResourceType = typeof(Resources.Global), UseDefaultRouteUrl = true, RemoveAllDefaultRouteValues = true, Order = 0)]
    public class SettingsController : Controller
    {
        private readonly IApplicationService _applicationService;

        public SettingsController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 0, TitleResourceName = "Recaptcha", TitleResourceType = typeof(Resources.Security), UseDefaultRouteUrl = true, RemoveAllDefaultRouteValues = true)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 0, TitleResourceName = "Recaptcha", TitleResourceType = typeof(Resources.Security), UseDefaultRouteUrl = true, RemoveAllDefaultRouteValues = true)]
        public IActionResult Recaptcha()
        {
            return View(new Recaptcha
            {
                RecaptchaScoreThreshold = _applicationService.GetSetting("RecaptchaScoreThreshold"),
                ReCaptchaSiteKey = _applicationService.GetSetting("ReCaptchaSiteKey"),
                ReCaptchaSecretKey = _applicationService.GetSetting("ReCaptchaSecretKey")
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DynamicPermission")]
        public IActionResult Recaptcha(Recaptcha model)
        {
            if (ModelState.IsValid)
            {
                _applicationService.SetSettings("RecaptchaScoreThreshold", model.RecaptchaScoreThreshold);
                _applicationService.SetSettings("ReCaptchaSiteKey", model.ReCaptchaSiteKey);
                _applicationService.SetSettings("ReCaptchaSecretKey", model.ReCaptchaSecretKey);

                _applicationService.UpdateCache();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "DynamicPermission")]
        [BreadCrumb(Order = 0, Title = "Recaptcha", UseDefaultRouteUrl = true, RemoveAllDefaultRouteValues = true)]
        public IActionResult RecurringJobs()
        {
            return View();
        }
    }
}
