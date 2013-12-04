using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AmvReporting.Infrastructure.Helpers
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Create a List with SelectListItems from the drop-down on the page.
        /// Use like this: _personRepository.All.ToSelectListItems(text => text.Name, value => value.PersonId)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">Items to be converted into dropdown</param>
        /// <param name="textSelector">Lambda selector for dropdown visible text</param>
        /// <param name="valueSelector">Lambda selector for dropdown option values</param>
        /// <param name="selectedValue">Object that should be selected in the list</param>
        /// <returns>List of SelectListItems ready to be fed to @Html.DropdownFor</returns>
        public static List<SelectListItem> ToSelectListItems<T>(
            this IEnumerable<T> items,
            Func<T, object> textSelector,
            Func<T, object> valueSelector,
            object selectedValue = null)
        {
            return items.OrderBy(item => textSelector(item).ToString())
                .Select(item => new SelectListItem
                {
                    Text = textSelector(item).ToString(),
                    Value = valueSelector(item).ToString(),
                    Selected = selectedValue != null && (valueSelector(item) == selectedValue)
                }).ToList();
        }
    }
}