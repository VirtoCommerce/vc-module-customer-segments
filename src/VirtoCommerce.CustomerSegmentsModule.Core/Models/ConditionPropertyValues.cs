using System;
using System.Collections.Generic;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CoreModule.Core.Conditions;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.CustomerSegmentsModule.Core.Models
{
    public class ConditionPropertyValues : ConditionTree
    {
        public string Salutation { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string DefaultLanguage { get; set; }
        public string TimeZone { get; set; }
        public string TaxPayerId { get; set; }
        public string PreferredDelivery { get; set; }
        public string PreferredCommunication { get; set; }

        public IList<string> Organizations { get; set; } = new List<string>();
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
    }
}
