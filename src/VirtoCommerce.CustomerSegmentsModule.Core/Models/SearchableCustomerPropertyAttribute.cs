using System;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Models
{
    /// <summary>
    /// Represents a model property, used to filter out not indexed properties 
    /// </summary>
    public class SearchableCustomerPropertyAttribute : Attribute
    {
        public string SearchablePropertyName { get; set; }

        public SearchableCustomerPropertyAttribute(string searchableName)
        {
            SearchablePropertyName = searchableName;
        }

        public SearchableCustomerPropertyAttribute()
        {
        }
    }
}
