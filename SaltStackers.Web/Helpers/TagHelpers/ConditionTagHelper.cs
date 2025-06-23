using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SaltStackers.Web.Helpers.TagHelpers
{
    [HtmlTargetElement(Attributes = "condition")]
    public class ConditionTagHelper : TagHelper
    {
        public bool Condition { get; set; } = true;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
            if (!Condition)
            {
                output.SuppressOutput();
            }
        }
    }
}
