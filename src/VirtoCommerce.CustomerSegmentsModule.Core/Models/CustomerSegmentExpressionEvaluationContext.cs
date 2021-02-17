using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CustomerModule.Core.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Models
{
    public class CustomerSegmentExpressionEvaluationContext : IEvaluationContext
    {
        public Contact Customer { get; set; }

        public virtual bool CustomerHasPropertyValues(CustomerSegmentConditionPropertyValues conditionPropertyValues)
        {
            var result = false;

            if (conditionPropertyValues != null)
            {
                result = CustomerHasDynamicPropertyValues(conditionPropertyValues.Properties) &&
                    CustomerHasModelPropertyValues(conditionPropertyValues);
            }

            return result;
        }

        protected virtual bool CustomerHasDynamicPropertyValues(IEnumerable<DynamicObjectProperty> properties)
        {
            var result = true;

            if (!properties.IsNullOrEmpty())
            {
                result = properties.All(property =>
                {
                    var hasPropertyValues = false;
                    var dynamicProperty = Customer.DynamicProperties
                        .FirstOrDefault(dp => dp.Name.EqualsInvariant(property.Name));
                    var propertyValues = property.Values.Where(v => v.Value != null).ToArray();

                    if (dynamicProperty != null && !propertyValues.IsNullOrEmpty())
                    {
                        var dynamicPropertyValues = dynamicProperty.Values.Where(v => v.Value != null).ToArray();
                        hasPropertyValues = propertyValues.Aggregate(false, (current, propertyValue) =>
                            current || dynamicPropertyValues.Any(dpv =>
                                dpv.Locale.EqualsInvariant(propertyValue.Locale) &&
                                dpv.Value.ToString().EqualsInvariant(propertyValue.Value.ToString())));
                    }

                    return hasPropertyValues;
                });
            }

            return result;
        }

        protected virtual bool CustomerHasModelPropertyValues(CustomerSegmentConditionPropertyValues propertyValues)
        {
            var result =
                Compare(propertyValues.BirthDate, Customer.BirthDate) &&
                Compare(propertyValues.DefaultLanguage, Customer.DefaultLanguage) &&
                Compare(propertyValues.FirstName, Customer.FirstName) &&
                Compare(propertyValues.LastName, Customer.LastName) &&
                Compare(propertyValues.MiddleName, Customer.MiddleName) &&
                Compare(propertyValues.FullName, Customer.FullName) &&
                Compare(propertyValues.Salutation, Customer.Salutation) &&
                Compare(propertyValues.TimeZone, Customer.TimeZone) &&
                Compare(propertyValues.TaxPayerId, Customer.TaxPayerId) &&
                Compare(propertyValues.PreferredDelivery, Customer.PreferredDelivery) &&
                Compare(propertyValues.PreferredCommunication, Customer.PreferredCommunication) &&
                Compare(propertyValues.AssociatedOrganizations, Customer.AssociatedOrganizations) &&
                Compare(propertyValues.Organizations, Customer.Organizations);

            return result;
        }

        private bool Compare(string ruleValue, string customerValue)
        {
            var result = true;

            if (!string.IsNullOrEmpty(ruleValue))
            {
                result = ruleValue.EqualsInvariant(customerValue);
            }

            return result;
        }

        private bool Compare(DateTime? ruleValue, DateTime? customerValue)
        {
            var result = true;

            if (ruleValue != null)
            {
                result = ruleValue == customerValue;
            }

            return result;
        }

        private bool Compare(IList<string> ruleValue, IList<string> customerValue)
        {
            var result = true;

            if (!ruleValue.IsNullOrEmpty())
            {
                result = customerValue?.Any(x => ruleValue.Contains(x)) ?? false;
            }

            return result;
        }
    }
}
