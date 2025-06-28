using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Operation.Kitchen;
using SaltStackers.Domain.Models.Membership;

namespace SaltStackers.Web.Areas.Service.Controllers;

/// <summary>
/// Everything about application settings
/// </summary>
[ApiController]
[Route("Service/[controller]")]
public class SettingController : ControllerBase
{
    private readonly IApplicationService _applicationService;
    private readonly IOperationService _operationService;
    private readonly UserManager<AspNetUser> _userManager;
    private readonly ILogService _logService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="applicationService">Application Service</param>
    /// <param name="operationService">Operation Service</param>
    /// <param name="userManager">User Manager</param>
    /// <param name="logService">Log Service</param>
    public SettingController(IApplicationService applicationService, IOperationService operationService,
        UserManager<AspNetUser> userManager, ILogService logService)
    {
        _applicationService = applicationService;
        _operationService = operationService;
        _userManager = userManager;
        _logService = logService;
    }

    /// <summary>
    /// Get Kitchens List
    /// </summary>
    /// <returns>List of Kitchens</returns>
    [HttpGet]
    [Route("[action]")]
    [AllowAnonymous]
    public async Task<ActionResult<List<KitchenApi>>> GetKitchens()
    {
        return new OkObjectResult(await _operationService.GetKitchensApiAsync(new KitchenFilters
        {
            PageSize = 10,
            Sort = "Title",
            Direction = "Asc"
        }));
    }

    /// <summary>
    /// Get Kitchen
    /// </summary>
    /// <returns>Kitchen</returns>
    [HttpGet]
    [Route("[action]")]
    [AllowAnonymous]
    public async Task<ActionResult<KitchenApi>> GetKitchen(int id)
    {
        var kitchen = await _operationService.GetKitchenApiAsync(id);

        if (kitchen != null && kitchen.Status.Equals("active", StringComparison.CurrentCultureIgnoreCase))
        {
            return new OkObjectResult(kitchen);
        }

        return new NotFoundObjectResult(string.Empty);
    }
}
