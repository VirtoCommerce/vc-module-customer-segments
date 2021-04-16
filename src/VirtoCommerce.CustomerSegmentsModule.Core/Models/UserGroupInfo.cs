namespace VirtoCommerce.CustomerSegmentsModule.Core.Models
{
    public class UserGroupInfo
    {
        public string UserGroup { get; set; }

        public bool IsDynamic { get; set; }

        public string DynamicRuleId { get; set; }

        public string DynamicRuleName { get; set; }
    }
}
