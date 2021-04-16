using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CoreModule.Core.Conditions;
using VirtoCommerce.CustomerModule.Core.Model.Search;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Models
{
    public class CustomerSegmentConditionPropertyValues : ConditionTree, ICanBuildSearchCriteria
    {
        [CustomerModelProperty]
        public string Salutation { get; set; }
        [CustomerModelProperty]
        public string FullName { get; set; }
        [CustomerModelProperty]
        public string FirstName { get; set; }
        [CustomerModelProperty]
        public string MiddleName { get; set; }
        [CustomerModelProperty]
        public string LastName { get; set; }

        [CustomerModelProperty]
        public DateTime? BirthDate { get; set; }
        [CustomerModelProperty]
        public string DefaultLanguage { get; set; }
        [CustomerModelProperty]
        public string TimeZone { get; set; }
        [CustomerModelProperty]
        public string TaxPayerId { get; set; }
        [CustomerModelProperty]
        public string PreferredDelivery { get; set; }
        [CustomerModelProperty]
        public string PreferredCommunication { get; set; }

        [CustomerModelProperty]
        public IList<string> Organizations { get; set; } = new List<string>();
        [CustomerModelProperty]
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
            // "DynamicProperyName1":"DynamicProperyValue" "DynamicProperyName2":"DynamicProperyValue1","DynamicProperyValue2" "ModelPropery1":"ModelProperyValue1"
            var searchPhrase = string.Join(' ', searchPraseParts);

            builder.WithSearchPhrase(searchPhrase);

            return builder.Build();
        }

        private IList<string> GetDynamicPropertiesParts()
        {
            var result = new List<string>();

            foreach (var property in Properties)
            {
                if (!property.Values.IsNullOrEmpty())
                {
                    var propertyName = QuoteValue(property.Name);
                    var values = property.Values.Select(x => GetSearchableValue(x.Value)).ToList();
                    var propertyValues = string.Join(',', values);
                    result.Add($"{propertyName}:{propertyValues}");
                }
            }

            return result;
        }

        private IList<string> GetModelPropertiesParts()
        {
            var result = new List<string>();

            var properties = GetProperties().Where(x => x.IsHaveAttribute(typeof(CustomerModelPropertyAttribute)));
            foreach (var property in properties)
            {
                var value = property.GetValue(this);
                if (value == null)
                {
                    continue;
                }

                IList<string> values = new List<string>();

                if (value.GetType().IsAssignableFromGenericList())
                {
                    foreach (var child in (IList)value)
                    {
                        values.Add(GetSearchableValue(child));
                    }
                }
                else
                {
                    values.Add(GetSearchableValue(value));
                }

                if (values.Any())
                {
                    var propertyName = GetSearchableName(property.Name);
                    var propertyValues = string.Join(',', values);
                    result.Add($"{propertyName}:{propertyValues}");
                }
            }

            return result;
        }

        private string QuoteValue(string value)
        {
            return $"\"{value}\"";
        }

        private string GetSearchableName(string propertyName)
        {
            var result = propertyName;

            if (propertyName.EqualsInvariant("organizations"))
            {
                result = "parentorganizations";
            }

            return QuoteValue(result);
        }

        private string GetSearchableValue(object value)
        {
            string result;

            if (value is DateTime dateTime)
            {
                result = dateTime.ToString("s");
            }
            else
            {
                result = value.ToString();
            }

            return QuoteValue(result);
        }


        public class CustomerModelPropertyAttribute : Attribute
        {
        }
    }
}
