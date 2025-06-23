namespace SaltStackers.Domain.Models.Nutrition
{
    public class IngredientTypeSubCategory
    {
        public int Id { get; set; }

        public int IngredientTypeId { get; set; }
        public virtual IngredientType? IngredientType { get; set; }

        public int IngredientSubCategoryId { get; set; }
        public virtual IngredientSubCategory? IngredientSubCategory { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
