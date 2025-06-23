using System.ComponentModel.DataAnnotations;

namespace SaltStackers.Common.Enums
{
    public enum LogicalOperator
    {
        [Display(Name = "And")]
        And = 1,

        [Display(Name = "Or")]
        Or = 2
    }
}
