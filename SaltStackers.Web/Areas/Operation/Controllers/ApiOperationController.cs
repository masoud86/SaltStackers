using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SaltStackers.Application.Interfaces;
using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Application.ViewModels.Operation.Kitchen;
using SaltStackers.Web.Helpers;

namespace SaltStackers.Web.Areas.Financial.Controllers
{
    [ApiController]
    public class ApiOperationController : ControllerBase
    {
        private readonly IOperationService _operationService;

        public ApiOperationController(IOperationService operationService,
            ICustomerService customerService, IFinancialService financialService)
        {
            _operationService = operationService;
        }

        private static List<ServiceError> ModelStateToServiceError(ModelStateDictionary modelstate)
        {
            var errors = new List<ServiceError>();
            foreach (var error in modelstate.Values.SelectMany(v => v.Errors))
            {
                errors.Add(new ServiceError
                {
                    Level = ErrorLevel.Blocker,
                    Description = error.ErrorMessage
                });
            }
            return errors;
        }
        
        [Log]
        [Authorize]
        [HttpGet("Api/Operation/GetRecipesByKitchen")]
        public async Task<IActionResult> GetRecipesByKitchen(int kitchenId)
        {
            return Ok(await _operationService.GetRecipesByKitchenAsync(kitchenId));
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Operation/AddRecipeToKitchen")]
        public async Task<IActionResult> AddRecipeToKitchen(int kitchenId, int recipeId)
        {
            return Ok(await _operationService.AddRecipeToKitchenAsync(kitchenId, recipeId));
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Operation/RemoveRecipeFromKitchen")]
        public async Task<IActionResult> RemoveRecipeFromKitchen(int kitchenId, int recipeId)
        {
            return Ok(await _operationService.RemoveRecipeFromKitchenAsync(kitchenId, recipeId));
        }

        [Log]
        [Authorize]
        [HttpGet("Api/Operation/GetKitchens")]
        public async Task<IActionResult> GetKitchens()
        {
            return Ok(await _operationService.GetKitchensModelAsync(new KitchenFilters
            {
                Direction = "ASC",
                PageSize = 100
            }));
        }
    }
}
