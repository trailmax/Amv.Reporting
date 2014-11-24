using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using AmvReporting.Infrastructure.CQRS;
using AmvReporting.Infrastructure.Dropdowns;


namespace AmvReporting.Infrastructure.PageGeneration
{
    public class DropDownBuilder<T, TValue> : IHtmlString
    {
        private readonly HtmlHelper<T> html;
        private readonly Expression<Func<T, TValue>> expression;
        private IEnumerable<SelectListItem> selectListItems;
        private IDropdownQuery query;
        private string defaultText = "Choose...";

        public DropDownBuilder(HtmlHelper<T> htmlHelper, Expression<Func<T, TValue>> providedExpression)
        {
            html = htmlHelper;
            expression = providedExpression;
        }


        public DropDownBuilder<T, TValue> ItemsFrom(IEnumerable<SelectListItem> items)
        {
            selectListItems = items;
            return this;
        }

        public DropDownBuilder<T, TValue> ItemsFrom(IDropdownQuery providedQuery)
        {
            query = providedQuery;
            return this;
        }

        public DropDownBuilder<T, TValue> DefaultText(String providedDefaultText)
        {
            defaultText = providedDefaultText;
            return this;
        }

        public MvcHtmlString Build()
        {
            selectListItems = selectListItems ?? new List<SelectListItem>();

            if (query != null)
            {
                var mediator = DependencyResolver.Current.GetService<IMediator>();
                selectListItems = mediator.Request(query);

                if (query is ICachedQuery)
                {
                    // MV: this is a bit of WTF. Every time cached drop-down is loaded, it's "selected" property is
                    // updated by the drop-down. And because we are using in-memory cache, we are not getting a copy 
                    // of the object, but we just get a reference to the object. And if the query result was messed 
                    // about in the previous execution, the cache value was also modified, which is bad.
                    // So here we need to copy over the items because we don't want to modify existing cache items
                    var copy = new List<SelectListItem>();
                    foreach (var item in selectListItems)
                    {
                        copy.Add(new SelectListItem() { Text = item.Text, Value = item.Value, Selected = item.Selected });
                    }
                    selectListItems = copy;
                }
            }

            var htmlAttributes = new Dictionary<String, object>();
            htmlAttributes.Add("class", "form-control");

            var dropDown = html.DropDownListFor(expression, selectListItems, defaultText, htmlAttributes);

            return dropDown;
        }


        public string ToHtmlString()
        {
            return Build().ToString();
        }
    }
}