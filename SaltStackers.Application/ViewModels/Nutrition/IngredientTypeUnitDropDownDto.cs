using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Nutrition
{
    public class IngredientTypeUnitDropDownDto
    {
        [Display(Name = "IngredientTypeUnit", ResourceType = typeof(Resources.Global))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        public int IngredientTypeUnitId { get; set; }
        public List<IngredientTypeUnitDto> IngredientTypeUnits { get; set; }

    }
}
