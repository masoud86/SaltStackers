using Microsoft.AspNetCore.Mvc;

namespace SaltStackers.Web.Controllers
{
    public class BaseController : Controller
    {
        private bool IsValidReturnUrl(string role, string? returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                return false;
            }

            returnUrl = returnUrl.ToLower().Trim();
            role = role.ToLower();

            if (role == "customer" || role == "partner")
            {
                return false;
            }

            if (!returnUrl.Contains("admin"))
            {
                return false;
            }

            return true;
        }

        public IActionResult RedirectLoggedInUser(string role, string? returnUrl = "")
        {
            if (IsValidReturnUrl(role, returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Dashboard", "Home", new { Area = "" });
        }
    }
}
