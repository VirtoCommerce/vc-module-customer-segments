using System;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Models
{
    /// <summary>
    /// Represents a model property, used to filter out not indexed properties 
    /// </summary>
    public class CustomerModelPropertyAttribute : Attribute
    {
        public string SearchableName { get; set; }

        public CustomerModelPropertyAttribute(string searchableName)
        {
            SearchableName = searchableName;
        }

        public CustomerModelPropertyAttribute()
        {
        }
    }
}
