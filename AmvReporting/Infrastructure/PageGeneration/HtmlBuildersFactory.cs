using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using AmvReporting.Infrastructure.Dropdowns;


namespace AmvReporting.Infrastructure.PageGeneration
{


    public class HtmlBuildersFactory<TModel>
    {
        public HtmlHelper<TModel> HtmlHelper { get; private set; }

        public HtmlBuildersFactory(HtmlHelper<TModel> htmlHelper)
        {
            HtmlHelper = htmlHelper;
        }


        public DropDownBuilder<TModel, TValue> Dropdown<TValue>(Expression<Func<TModel, TValue>> expression, IDropdownQuery query)
        {
            var builder = new DropDownBuilder<TModel, TValue>(HtmlHelper, expression)
                    .ItemsFrom(query);

            return builder;
        }


        public DropDownBuilder<TModel, TValue> Dropdown<TValue>(Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectListItems)
        {
            var builder = new DropDownBuilder<TModel, TValue>(HtmlHelper, expression)
                    .ItemsFrom(selectListItems);

            return builder;
        }
    }
}