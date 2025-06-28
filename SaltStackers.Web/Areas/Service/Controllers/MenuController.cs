using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Api;
using SaltStackers.Application.ViewModels.Nutrition;
using SaltStackers.Application.ViewModels.Nutrition.Package;
using SaltStackers.Domain.Models.Membership;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SaltStackers.Web.Areas.Service.Controllers
{
    /// <summary>
    /// Everything about menu
    /// </summary>
    [ApiController]
    //[Route("[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly INutritionService _nutritionService;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public MenuController(INutritionService nutritionService, UserManager<AspNetUser> userManager,
            IConfiguration configuration, ITokenService tokenService)
        {
            _nutritionService = nutritionService;
            _userManager = userManager;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        ///// <summary>
        ///// All menu items
        ///// </summary>
        ///// <remarks>
        ///// Paging with default page size of 10
        ///// </remarks>
        ///// <param name="query">Seach term</param>
        ///// <param name="diet">Filter by diet</param>
        ///// <param name="tags">Filter by tags (comma separated)</param>
        ///// <param name="prepDays">Filter by prep days (comma separated)</param>
        ///// <param name="page">Page number</param>
        ///// <param name="pageSize">Page size</param>
        ///// <param name="sort">Sort column</param>
        ///// <param name="direction">Sort direction</param>
        ///// <param name="kitchen">Kitchen Id</param>
        ///// <returns>List of items</returns>
        ///// <response code="200">Successful operation</response>
        ///// <response code="400">Bad Request</response>
        //[HttpGet]
        ////[Route("[action]")]
        //[AllowAnonymous]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        //public async Task<ActionResult<MenuItems>> Items(string? query, string? diet, string? tags, string? prepDays, int page = 1, int pageSize = 10, string? sort = "Title", string? direction = "Asc", int kitchen = 1)
        //{
        //    var ownerId = "";
        //    if (diet != null && diet == "personal" && User != null)
        //    {
        //        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);

        //        if (!string.IsNullOrEmpty(authorizationHeader))
        //        {
        //            if (_tokenService.IsExpiredToken(authorizationHeader.ToString().Replace("Bearer ", string.Empty)))
        //            {
        //                return Unauthorized("Invalid token");
        //            }
        //        }

        //        var claim = User.Claims.FirstOrDefault(p => p.Type == "name");
        //        if (claim != null)
        //        {
        //            var username = claim.Value;
        //            var user = await _userManager.FindByNameAsync(username);
        //            ownerId = user?.Id;
        //        }
        //    }
        //    return new OkObjectResult(await _nutritionService.GetMenuItemsAsync(new MenuItemFilters
        //    {
        //        KitchenId = kitchen,
        //        Page = page,
        //        PageSize = pageSize,
        //        Sort = !string.IsNullOrEmpty(sort) && sort != "null" ? sort : "",
        //        Direction = !string.IsNullOrEmpty(direction) && direction != "null" ? direction : "",
        //        Diet = !string.IsNullOrEmpty(diet) && diet != "null" ? diet : null,
        //        Tags = !string.IsNullOrEmpty(tags) ? tags.Split(',') : null,
        //        PrepDays = !string.IsNullOrEmpty(prepDays) && prepDays != "null" ? prepDays.Split(',').Select(int.Parse).Select(p => (DayOfWeek)p).ToList() : null,
        //        Query = !string.IsNullOrEmpty(query) && query != "null" ? query : "",
        //        OwnerId = !string.IsNullOrEmpty(ownerId) ? ownerId : null
        //    }));
        //}

        ///// <summary>
        ///// Item details
        ///// </summary>
        ///// <param name="code">Item code</param>
        ///// <param name="kitchenId">Kitchen Id</param>
        ///// <returns>Item details</returns>
        ///// <response code="200">Successful operation</response>
        ///// <response code="400">Bad Request</response>
        //[HttpGet]
        ////[Route("[action]")]
        //public async Task<ActionResult<ItemDetails>> ItemDetails(string code, int kitchenId)
        //{
        //    if (string.IsNullOrEmpty(code))
        //    {
        //        return BadRequest();
        //    }
        //    var details = await _nutritionService.GetRecipeDetailsApi(code, kitchenId);
        //    if (details == null || details.Id == 0)
        //    {
        //        return new NotFoundObjectResult(details);
        //    }
        //    return new OkObjectResult(details);
        //}

        ///// <summary>
        ///// Item details
        ///// </summary>
        ///// <param name="code">Item code</param>
        ///// <returns>Item details</returns>
        ///// <response code="200">Successful operation</response>
        ///// <response code="400">Bad Request</response>
        //[HttpGet]
        ////[Route("[action]")]
        //public async Task<ActionResult<PackageDetails>> PackageDetails(string code)
        //{
        //    if (string.IsNullOrEmpty(code))
        //    {
        //        return BadRequest();
        //    }
        //    var details = await _nutritionService.GetPackageAsync(code);
        //    if (details == null)
        //    {
        //        return new NotFoundObjectResult(details);
        //    }
        //    return new OkObjectResult(details);
        //}

        ///// <summary>
        ///// Calculate recipe variables
        ///// </summary>
        ///// <remarks>
        ///// Total price and total nutrition facts can be changed based on ingrdients and their amounts
        ///// </remarks>
        ///// <param name="code">Recipe Code</param>
        ///// <param name="changes">Recipe Changes</param>
        ///// <returns>Recipe variables model</returns>
        //[HttpPost]
        ////[Route("[action]")]
        //public async Task<ActionResult<RecipeVariables>> CalculateRecipe(string code, RecipeChanges changes)
        //{
        //    return new OkObjectResult(await _nutritionService.CalculateRecipeAsync(code, changes, true));
        //}
    }
}
