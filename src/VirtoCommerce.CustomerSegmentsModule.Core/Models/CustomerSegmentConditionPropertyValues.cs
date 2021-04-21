using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CoreModule.Core.Conditions;
using VirtoCommerce.CustomerModule.Core.Model.Search;
using VirtoCommerce.CustomerSegmentsModule.Core.Extensions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Models
{
    public class CustomerSegmentConditionPropertyValues : ConditionTree, ICanBuildSearchCriteria
    {
        [SearchableCustomerProperty]
        public string Salutation { get; set; }
        [SearchableCustomerProperty]
        public string FullName { get; set; }
        [SearchableCustomerProperty]
        public string FirstName { get; set; }
        [SearchableCustomerProperty]
        public string MiddleName { get; set; }
        [SearchableCustomerProperty]
        public string LastName { get; set; }

        [SearchableCustomerProperty]
        public DateTime? BirthDate { get; set; }
        [SearchableCustomerProperty]
        public string DefaultLanguage { get; set; }
        [SearchableCustomerProperty]
        public string TimeZone { get; set; }
        [SearchableCustomerProperty]
        public string TaxPayerId { get; set; }
        [SearchableCustomerProperty]
        public string PreferredDelivery { get; set; }
        [SearchableCustomerProperty]
        public string PreferredCommunication { get; set; }

        [SearchableCustomerProperty("parentorganizations")]
        public IList<string> Organizations { get; set; } = new List<string>();
        [SearchableCustomerProperty]
        public IList<string> AssociatedOrganizations { get; set; } = new List<string>();

        public ICollection<DynamicObjectProperty> Properties { get; set; } = new List<DynamicObjectProperty>();

        public override bool IsSatisfiedBy(IEvaluationContext context)
        {
            var result = false;

            if (context is CustomerSegmentExpressionEvaluationContext evaluationContext)
            {
                return evaluationContext.CustomerHasPropertyValues(this);
            }

            return result;
        }

        public virtual MembersSearchCriteria BuildSearchCriteria(IMemberSearchCriteriaBuilder builder)
        {
            var searchPraseParts = GetDynamicPropertiesParts();
            searchPraseParts.AddRange(GetModelPropertiesParts());

            // creates a search phrase query like:
            // "DynamicProperyName1":"DynamicProperyValue" "ArrayDynamicProperyName":"DynamicProperyValue1","DynamicProperyValue2" "ModelPropery1":"ModelProperyValue1"
            var searchPhrase = string.Join(' ', searchPraseParts);

            builder.WithSearchPhrase(searchPhrase);

            return builder.Build();
        }

        private IList<string> GetDynamicPropertiesParts()
        {
            var result = Properties
                .Where(x => !x.Values.IsNullOrEmpty())
                .Select(property =>
                {
                    var propertyValues = property.Values.Select(x => x.Value);
                    return GetSearchQueryPart(property.Name, propertyValues);
                })
                .ToList();

            return result;
        }

        private IList<string> GetModelPropertiesParts()
        {
            var result = new List<string>();

            var properties = GetProperties().Where(x => x.IsHaveAttribute(typeof(SearchableCustomerPropertyAttribute)));
            foreach (var property in properties)
            {
                var propertyValues = GetPropertyValues(property);

                if (propertyValues.Any())
                {
                    var attribute = property.GetCustomAttributes<SearchableCustomerPropertyAttribute>().FirstOrDefault();
                    var propertyName = !string.IsNullOrEmpty(attribute?.SearchablePropertyName) ? attribute.SearchablePropertyName : property.Name;

                    var queryPart = GetSearchQueryPart(propertyName, propertyValues);
                    result.Add(queryPart);
                }
            }

            return result;
        }

        private IList<object> GetPropertyValues(PropertyInfo property)
        {
            var result = new List<object>();
            var value = property.GetValue(this);

            if (value != null)
            {
                if (value.GetType().IsAssignableFromGenericList())
                {
                    foreach (var child in (IList)value)
                    {
                        result.Add(child);
                    }
                }
                else
                {
                    result.Add(value);
                }
            }

            return result;
        }

        private string GetSearchQueryPart(string propertyName, IEnumerable<object> propertyValues)
        {
            var quotedName = QuoteValue(propertyName);
            var quotedValues = propertyValues.Select(x => GetSearchableValue(x));
            var joinedValue = string.Join(',', quotedValues);

            return $"{quotedName}:{joinedValue}";
        }

        private string GetSearchableValue(object value)
        {
            if (value is DateTime dateTime)
            {
                return QuoteValue(dateTime.ToString("s"));
            }

            return QuoteValue(value.ToString());
        }

        private string QuoteValue(string value)
        {
            return $"\"{value}\"";
        }
    }
}
