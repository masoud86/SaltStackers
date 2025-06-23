using SaltStackers.Application.ViewModels.Nutrition;
using SaltStackers.Domain.Models.Nutrition;
using LinqKit;

namespace SaltStackers.Application.Filters
{
    public static class MarketingFilter
    {
        public static ExpressionStarter<Tag> ToExpression(TagFilters filter)
        {
            var predicate = PredicateBuilder.New<Tag>(_ => true);

            if (!string.IsNullOrEmpty(filter.Query))
            {
                predicate.And(p =>
                    p.Title.ToLower().Contains(filter.Query) ||
                    p.Permalink.ToLower().Contains(filter.Query));
            }

            return predicate;
        }
    }
}
