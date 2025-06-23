namespace SaltStackers.Domain.Models.Nutrition
{
    public class Food
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime CreateDateTime { get; set; }

        public int ProfitMargin { get; set; }

        public virtual List<Recipe>? Recipes { get; set; }

        public virtual List<FoodAttachment>? Attachments { get; set; }
    }
}
