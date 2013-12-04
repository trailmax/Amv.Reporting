using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace AmvReporting.Infrastructure.Helpers
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString AjaxDeleteButton<T>(this UrlHelper urlHelper, ActionResult actionResult, T data, Expression<Func<T, String>> selector)
        {
            var tagBuilder = new TagBuilder("button");
            tagBuilder.SetInnerText("Delete");
            tagBuilder.AddCssClass("btn btn-default delete-by-ajax");
            tagBuilder.MergeAttribute("title", "Delete");

            tagBuilder.MergeAttribute("data-id", ExpressionHelper.PropertyValue(data, selector));
            tagBuilder.MergeAttribute("data-parameter-name", ExpressionHelper.PropertyName(selector));

            tagBuilder.MergeAttribute("data-url", urlHelper.Action(actionResult));

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }
}