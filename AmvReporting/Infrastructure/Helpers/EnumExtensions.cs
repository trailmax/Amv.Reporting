using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace AmvReporting.Infrastructure.Helpers
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Create a list with SelectListItems from an Enum.
        /// Takes DisplayAttributes first if present, otherwise takes string value of enum and separates with spaces words.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enum"></param>
        /// <param name="selected">Value for the selected element</param>
        /// <param name="useNumbersAsValues"></param>
        /// <returns></returns>
        public static List<SelectListItem> ToSelectListItems<T>(this T @enum, string selected = "", bool useNumbersAsValues = false) where T : struct
        {
            var items = ConstructItemsList<T>();

            return items.Select(pair => new SelectListItem()
            {
                Text = pair.Value,
                Value = useNumbersAsValues ? pair.Key.ToString() : Enum.ToObject(@enum.GetType(), pair.Key).ToString(),
                Selected = (pair.Value == selected)
            }).ToList();
        }


        private static Dictionary<object, string> ConstructItemsList<T>() where T : struct
        {
            var source = Enum.GetValues(typeof(T));

            var items = new Dictionary<object, string>();

            var displayAttributeType = typeof(DisplayAttribute);

            foreach (var value in source)
            {
                var field = value.GetType().GetField(value.ToString());

                if (field == null)
                {
                    continue;
                }

                var attribute = (DisplayAttribute)field.GetCustomAttributes(displayAttributeType, false).FirstOrDefault();
                if (attribute != null && attribute.GetName() != null)
                {
                    items.Add((int)value, attribute.GetName());
                }
                else
                {
                    // in case Description.Name attribute is not available, we fall back to the default name
                    items.Add((int)value, value.ToString().ToSeparatedWords());
                }
            }
            return items;
        }
    }
}