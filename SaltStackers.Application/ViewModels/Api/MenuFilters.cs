using SaltStackers.Application.ViewModels.Base;
using SaltStackers.Application.ViewModels.Nutrition;

namespace SaltStackers.Application.ViewModels.Api
{
    public class MenuFilters
    {
        public List<DietApi>? Diets { get; set; }

        public List<TagApi>? Tags { get; set; }

        public List<Day>? CookingDays { get; set; }
    }
}
