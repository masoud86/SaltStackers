using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SaltStackers.Web.Helpers.TagHelpers
{
    //<div class="card">
    //  <div class="card-header text-center bg-primary">
    //    <span class="fa-stack fa-2x">
    //      <i class="fas fa-circle fa-stack-2x text-light"></i>
    //      <i class="{{Icon}} fa-stack-1x fa-inverse text-primary"></i>
    //    </span>
    //  </div>
    //  <div class="card-body">
    //    <h3 class="card-title">
    //      <a class="stretched-link text-primary" href="{{Url}}">{{Title}}</a>
    //    </h3>
    //    <p class="card-text">{{Description}}</p>
    //  </div>
    //</div>
    [HtmlTargetElement("card-link", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class CardLinkTagHelper : TagHelper
    {
        public string Url { get; set; } = "#";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "N/A";
        public string Icon { get; set; } = "fas fa-bars";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "card");

            var iconWrapper = new TagBuilder("span");
            iconWrapper.AddCssClass("fa-stack fa-2x position-absolute top-0 end-0 mt-2 me-1");

            var iconStack = new TagBuilder("i");
            iconStack.AddCssClass("fas fa-circle fa-stack-2x text-secondary text-opacity-25");

            var icon = new TagBuilder("i");
            icon.AddCssClass($"{Icon} fa-stack-1x fa-inverse text-secondary");

            var cardBody = new TagBuilder("div");
            cardBody.AddCssClass("card-body");

            var cardTitle = new TagBuilder("h2");
            cardTitle.AddCssClass("text-secondary");

            var link = new TagBuilder("a");
            link.AddCssClass("stretched-link text-primary");
            link.Attributes.Add("href", Url);
            link.InnerHtml.Append(Title);

            var paragraph = new TagBuilder("p");
            paragraph.AddCssClass("card-text");
            paragraph.InnerHtml.Append(Description);

            iconWrapper.InnerHtml.AppendHtml(iconStack).AppendHtml(icon);
            cardTitle.InnerHtml.AppendHtml(link);
            cardBody.InnerHtml.AppendHtml(iconWrapper).AppendHtml(cardTitle).AppendHtml(paragraph);
            output.Content.AppendHtml(cardBody);
        }
    }
}
