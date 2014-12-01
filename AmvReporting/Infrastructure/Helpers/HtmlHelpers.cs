using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using AmvReporting.Infrastructure.PageGeneration;


namespace AmvReporting.Infrastructure.Helpers
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString AjaxDeleteButton<T, TResult>(this UrlHelper urlHelper, ActionResult actionResult, T data, Expression<Func<T, TResult>> selector)
        {
            return AjaxDeleteButton(urlHelper, actionResult, ExpressionHelper.PropertyValue(data, selector),
                ExpressionHelper.PropertyName(selector));
        }


        public static MvcHtmlString AjaxDeleteButton(this UrlHelper urlHelper, ActionResult actionResult, object data, String parameterName)
        {
            var tagBuilder = new TagBuilder("button");
            tagBuilder.SetInnerText("Delete");
            tagBuilder.AddCssClass("btn btn-default btn-xs delete-by-ajax");
            tagBuilder.MergeAttribute("title", "Delete");

            tagBuilder.MergeAttribute("data-id", data.ToString());
            tagBuilder.MergeAttribute("data-parameter-name", parameterName);

            tagBuilder.MergeAttribute("data-url", urlHelper.Action(actionResult));

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }

        public static String ElementId<T, TResult>(this HtmlHelper<T> htmlHelper, Expression<Func<T, TResult>> selector)
        {
            return ExpressionHelper.PropertyName(selector);
        }

        public static HtmlBuildersFactory<TModel> Domain<TModel>(this HtmlHelper<TModel> html)
        {
            return new HtmlBuildersFactory<TModel>(html);
        }
    }
}