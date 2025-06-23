using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Application.ViewModels.Nutrition
{
    public class IngredientTypeDropDownDto
    {
        [Display(Name = "IngredientType", ResourceType = typeof(Resources.Global))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.Error))]
        public int IngredientTypeId { get; set; }
        public List<IngredientTypeDto> IngredientTypes { get; set; }

    }
}
