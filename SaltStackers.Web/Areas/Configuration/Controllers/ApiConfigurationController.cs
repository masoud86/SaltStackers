using SaltStackers.Application.Interfaces;
using SaltStackers.Web.Helpers;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SaltStackers.Web.Areas.Configuration.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiConfigurationController : ControllerBase
    {
        private readonly INutritionService _nutritionService;
        private readonly IFinancialService _financialService;
        private readonly IOperationService _operationService;

        public ApiConfigurationController(INutritionService nutritionService,
            IFinancialService financialService, IOperationService operationService)
        {
            _nutritionService = nutritionService;
            _financialService = financialService;
            _operationService = operationService;
        }

        [Log]
        [Authorize]
        [HttpPost("Api/Configuration/UpdatePrices")]
        public IActionResult UpdatePrices()
        {
            RecurringJob.AddOrUpdate("UpdatePrices",
                () => _nutritionService.UpdateRecipesPrice(),
                Cron.Hourly);
            return Ok();
        }
    }
}
