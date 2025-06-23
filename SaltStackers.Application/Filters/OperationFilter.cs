using LinqKit;
using SaltStackers.Application.ViewModels.Operation;
using SaltStackers.Application.ViewModels.Operation.Kitchen;
using SaltStackers.Common.Enums;
using SaltStackers.Domain.Models.Operation;

namespace SaltStackers.Application.Filters;

public static class OperationFilter
{

    public static ExpressionStarter<OverheadCost> ToExpression(OverheadCostFilters filter)
    {
        var predicate = PredicateBuilder.New<OverheadCost>(_ => true);

        if (!string.IsNullOrEmpty(filter.Query))
        {
            predicate.And(p => p.Title.ToLower().Contains(filter.Query));
        }

        return predicate;
    }

    public static ExpressionStarter<OverheadCost> ToExpression(OverheadCategory category)
    {
        var predicate = PredicateBuilder.New<OverheadCost>(_ => true);
        if (category != OverheadCategory.All)
        {
            predicate.And(p => p.Category == category);
        }
        return predicate;
    }

    public static ExpressionStarter<Kitchen> ToExpression(KitchenFilters filter)
    {
        var predicate = PredicateBuilder.New<Kitchen>(_ => true);

        if (!filter.ShowAll)
        {
            predicate.And(p => p.Status != PartnerStatus.Inactive);
        }

        if (!string.IsNullOrEmpty(filter.Query))
        {
            predicate.And(p =>
                p.Title.Contains(filter.Query, StringComparison.CurrentCultureIgnoreCase));
        }

        return predicate;
    }
}
