namespace SaltStackers.Application.ViewModels.Nutrition
{
    public class IngredientTypeSubCategoryDto
    {
        public int Id { get; set; }

        public int IngredientTypeId { get; set; }
        public IngredientTypeDto? IngredientType { get; set; }

        public int IngredientSubCategoryId { get; set; }
        public IngredientSubCategoryDto? IngredientSubCategory { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
