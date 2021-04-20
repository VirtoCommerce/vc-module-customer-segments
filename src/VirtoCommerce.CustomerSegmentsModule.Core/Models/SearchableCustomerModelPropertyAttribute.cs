using System;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Models
{
    /// <summary>
    /// Represents a model property, used to filter out not indexed properties 
    /// </summary>
    public class SearchableCustomerModelPropertyAttribute : Attribute
    {
        public string SearchableName { get; set; }

        public SearchableCustomerModelPropertyAttribute(string searchableName)
        {
            SearchableName = searchableName;
        }

        public SearchableCustomerModelPropertyAttribute()
        {
        }
    }
}
