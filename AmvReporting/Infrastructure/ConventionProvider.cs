using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AmvReporting.Infrastructure.Helpers;

namespace AmvReporting.Infrastructure
{
    /// <summary>
    /// Cleverness from Jimmy Bogard. 
    /// Whenever a model property does not have Display Attribute, we add it here: separate words with spaces.
    /// </summary>
    public class ConventionProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(
            IEnumerable<Attribute> attributes,
            Type containerType,
            Func<object> modelAccessor,
            Type modelType,
            string propertyName)
        {
            var meta = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
            if (meta.DisplayName == null && meta.PropertyName != null)
            {
                meta.DisplayName = meta.PropertyName;
                if (meta.ModelType.FullName.Contains("Int") &&
                    meta.DisplayName.Substring(meta.DisplayName.Length - 2, 2).Equals("Id", StringComparison.Ordinal))
                {
                    meta.DisplayName = meta.DisplayName.Remove(meta.DisplayName.Length - 2);
                }
                meta.DisplayName = meta.DisplayName.ToSeparatedWords();
            }
            return meta;
        }
    }
}