using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Log;
using SaltStackers.Application.ViewModels.Operation.Kitchen;
using SaltStackers.Application.ViewModels.Settings;
using SaltStackers.Application.ViewModels.Settings.Alert;
using SaltStackers.Domain.Models.Membership;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SaltStackers.Api.Controllers;

/// <summary>
/// Everything about application settings
/// </summary>
[ApiController]
[Route("[controller]")]
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
    /// Get customer app current version
    /// </summary>
    /// <returns>Current Version</returns>
    [HttpGet]
    [Route("[action]")]
    public ActionResult<string> GetCurrentVersion()
    {
        return new OkObjectResult(_applicationService.GetSetting("CustomerAppVersion"));
    }

    /// <summary>
    /// Temporary API to set customer app current version
    /// </summary>
    /// <param name="version">Current Version</param>
    /// <returns>Ok Response</returns>
    [HttpGet]
    [Route("[action]")]
    public ActionResult SetCurrentVersion(string version)
    {
        _applicationService.SetSettings("CustomerAppVersion", version);
        _applicationService.UpdateCache();
        return Ok();
    }

    /// <summary>
    /// Get Zones List
    /// </summary>
    /// <param name="kitchenId">Kitchen ID</param>
    /// <returns>List of Zones</returns>
    [HttpGet]
    [Route("[action]")]
    public async Task<ActionResult<List<ZoneApi>>> GetZones(int kitchenId)
    {
        return new OkObjectResult(await _applicationService.GetZonesByKitchenAsync(kitchenId));
    }

    /// <summary>
    /// Get Alerts List
    /// </summary>
    /// <returns>List of alerts</returns>
    [HttpGet]
    [Route("[action]")]
    [AllowAnonymous]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<ActionResult<List<AlertApi>>> GetAlerts()
    {
        string? userId = null;
        if (User != null)
        {
            var claim = User.Claims.FirstOrDefault(p => p.Type == "name");
            if (claim != null)
            {
                var username = claim.Value;
                var user = await _userManager.FindByNameAsync(username);
            }
        }

        return new OkObjectResult(await _applicationService.GetAlertsAsync(userId));
    }

    /// <summary>
    /// Set Alert to Seen
    /// </summary>
    /// <param name="alertId">Alert Id</param>
    /// <returns>Action Result</returns>
    [HttpPost]
    [Route("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<ActionResult> AlertSeen(int alertId)
    {
        var username = User.Claims.First(p => p.Type == "name").Value;
        var user = await _userManager.FindByNameAsync(username);
        await _applicationService.SeenAlertAsync(user.Id, alertId);
        return Ok();
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
