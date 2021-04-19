namespace VirtoCommerce.CustomerSegmentsModule.Core.Models
{
    /// <summary>
    /// Represents a customer UserGroup, both assigned to the customer and dynamically evaluated
    /// </summary>
    public class UserGroupInfo
    {
        public string UserGroup { get; set; }

        public bool IsDynamic { get; set; }

        public string DynamicRuleId { get; set; }

        public string DynamicRuleName { get; set; }
    }
}
