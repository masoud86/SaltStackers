using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using reCAPTCHA.AspNetCore;
using SaltStackers.Application.Helpers;
using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Customer;
using SaltStackers.Application.ViewModels.General;
using SaltStackers.Application.ViewModels.Membership;
using SaltStackers.Application.ViewModels.Nutrition;
using SaltStackers.Application.ViewModels.Operation.Kitchen;
using SaltStackers.Common.Helper;
using SaltStackers.Web.Models;
using System.Diagnostics;
using System.Globalization;

namespace SaltStackers.Web.Controllers;

public class HomeController : BaseController
{
    private readonly ILogService _logService;
    private readonly IMembershipService _membershipService;
    private readonly IRecaptchaService _recaptcha;
    private readonly double _recaptchaScoreThreshold;
    private readonly bool _isRecaptchaEnable;
    private readonly IFinancialService _financialService;
    private readonly IApplicationService _applicationService;
    private readonly INutritionService _nutritionService;
    private readonly ICustomerService _customerService;
    private readonly IOperationService _operationService;
    private readonly IHostEnvironment _hostEnvironment;


    public HomeController(ILogService logService, IMembershipService membershipService,
        IRecaptchaService recaptcha, IApplicationService applicationService,
        IConfiguration configuration, IFinancialService financialService,
        INutritionService nutritionService, ICustomerService customerService,
        IOperationService operationService, IHostEnvironment hostEnvironment)
    {
        _logService = logService;
        _membershipService = membershipService;
        _recaptcha = recaptcha;
        _financialService = financialService;
        _applicationService = applicationService;
        _nutritionService = nutritionService;
        _customerService = customerService;
        _operationService = operationService;
        _hostEnvironment = hostEnvironment;

        _isRecaptchaEnable = configuration.GetValue<bool>("RecaptchaActive");
        _recaptchaScoreThreshold = double.Parse(_applicationService.GetSetting("RecaptchaScoreThreshold"),
            CultureInfo.CreateSpecificCulture("en-US"));
    }

    private async Task<bool> IsValidRecaptcha(HttpRequest request)
    {
#if !DEBUG
        var recaptcha = await _recaptcha.Validate(request);
        if (_isRecaptchaEnable &&
            (!recaptcha.success || Math.Abs(recaptcha.score) > 0 && recaptcha.score < _recaptchaScoreThreshold))
        {
            return false;
        }
#endif
        return true;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? returnUrl)
    {
        if (_membershipService.IsSignedIn(User))
        {
            var isBlocked = await _membershipService.IsBlockedAsync(User.Id());
            if (!isBlocked)
            {
                return RedirectLoggedInUser(User.Role(), returnUrl);
            }
        }

        var model = new Login
        {
            ReturnUrl = returnUrl
        };

        ViewData["ReturnUrl"] = returnUrl;

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(Login model, string? returnUrl)
    {
        if (!await IsValidRecaptcha(Request))
        {
            ModelState.AddModelError("Recaptcha", Resources.Error.CaptchaError);
            return View(model);
        }

        if (_membershipService.IsSignedIn(User))
        {
            var isBlocked = await _membershipService.IsBlockedAsync(User.Id());
            if (!isBlocked)
            {
                return RedirectLoggedInUser(User.Role(), returnUrl);
            }
        }

        ViewData["ReturnUrl"] = returnUrl;

        if (ModelState.IsValid)
        {
            var loggedInUser = await _membershipService.FindUserByNameAsync(model.Username);

            if (loggedInUser == null)
            {
                ModelState.AddModelError("", "Username or password is incorrect.");
                return View(model);
            }

            var role = await _membershipService.GetRoleByUserIdAsync(loggedInUser.Id);
            if (role == "Customer" || role == "Partner")
            {
                ModelState.AddModelError("", "Username or password is incorrect.");
                return View(model);
            }

            var signinResult = await _membershipService.LoginAsync(model);

            if (signinResult.Succeeded)
            {
                return RedirectLoggedInUser(role, returnUrl);
            }

            if (signinResult.IsLockedOut)
            {
                ModelState.AddModelError("", "Your account is locked");
                return View(model);
            }

            ModelState.AddModelError("", "Username or password is incorrect.");
        }
        return View(model);
    }

    [HttpGet]
    public ViewResult Errors() => View("Error");

    [Route("404")]
    public IActionResult PageNotFound()
    {
        string originalPath = "unknown";
        if (HttpContext.Items.ContainsKey("originalPath"))
        {
            originalPath = HttpContext.Items["originalPath"] as string;
        }
        return View();
    }

    [HttpGet]
    public IActionResult AccessDenied(string returnUrl)
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Error()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
        if (exception != null)
        {
            var path = HttpContext.Features.Get<IExceptionHandlerPathFeature>().Path;
        }
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignOutAsync()
    {
        await _logService.AddUserLogAsync(User.Id(), "User", "LoggedOff", "", Request.GetClientInfo());
        _membershipService.SignOut();
        try
        {
            if (TempData.ContainsKey("TOTP"))
                TempData["TOTP"].ToString();
        }
        catch (Exception e)
        {
            var path = HttpContext.Features.Get<IExceptionHandlerPathFeature>().Path;
        }

        var returnUrl = Url.Action("Index", "Home", new { Area = "" });
        var isAjaxCall = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        if (isAjaxCall)
        {
            return Json(new { Url = returnUrl });
        }
        return RedirectToAction("Index", "Home", new { Area = "" });
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string userName, string token)
    {
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(token))
        {
            return NotFound();
        }
        var user = await _membershipService.FindUserByNameAsync(userName);
        if (user == null)
        {
            return NotFound();
        }
        var result = await _membershipService.ConfirmEmailAsync(user, token);

        return Content(result.Succeeded ? "Email Confirmed" : "Email Not Confirmed");
    }

    [Authorize]
    [HttpGet("Dashboard")]
    [BreadCrumb(Title = "Dashboard", UseDefaultRouteUrl = false, RemoveAllDefaultRouteValues = false, Order = 0)]
    public async Task<IActionResult> Dashboard()
    {
        return View(new Dashboard
        {
            KitchensCount = await _operationService.CountKitchenAsync(),
            FoodsCount = await _nutritionService.CountFoodsAsyc(new FoodFilters()),
            RecipesCount = await _nutritionService.CountRecipesAsyc(new RecipeFilters()),
            CustomersCount = await _customerService.CountCustomersAsync(new CustomerFilters()),
            IngredientsCount = await _nutritionService.CountIngredientsAsyc(),
            Kitchens = await _operationService.GetKitchensAsync(new KitchenFilters { PageSize = 10, ShowAll = true })
        });
    }

    [HttpGet("ReleaseNotes")]
    [BreadCrumb(Title = "Release Notes", UseDefaultRouteUrl = false, RemoveAllDefaultRouteValues = false, Order = 0)]
    public IActionResult ReleaseNotes()
    {
        var projectRootPath = _hostEnvironment.ContentRootPath;
        var releaseNotesContent = System.IO.File.ReadAllText(Path.Combine(projectRootPath, "ReleaseNotes.md"));
        return View(new ReleaseNotes { Content = HtmlHelper.MarkdownToHtml(releaseNotesContent) });
    }
}
