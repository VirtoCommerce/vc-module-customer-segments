using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.CustomerSegmentsModule.Core
{
    public static class ModuleConstants
    {
        public const int MaxAllowedSegments = 1000;
        public const int MaxActiveSegments = 20;

        public static class Security
        {
            public static class Permissions
            {
                public const string Access = "customerSegments:access";
                public const string Create = "customerSegments:create";
                public const string Read = "customerSegments:read";
                public const string Update = "customerSegments:update";
                public const string Delete = "customerSegments:delete";

                public static string[] AllPermissions { get; } = { Read, Create, Access, Update, Delete };
            }
        }

        public static class Settings
        {
            public static class General
            {
                public static SettingDescriptor VirtoCommerceCustomerSegmentsModuleEnabled { get; } = new SettingDescriptor
                {
                    Name = "VirtoCommerceCustomerSegmentsModule.VirtoCommerceCustomerSegmentsModuleEnabled",
                    GroupName = "VirtoCommerceCustomerSegmentsModule|General",
                    ValueType = SettingValueType.Boolean,
                    DefaultValue = false
                };

                //public static SettingDescriptor VirtoCommerceCustomerSegmentsModulePassword { get; } = new SettingDescriptor
                //{
                //    Name = "VirtoCommerceCustomerSegmentsModule.VirtoCommerceCustomerSegmentsModulePassword",
                //    GroupName = "VirtoCommerceCustomerSegmentsModule|Advanced",
                //    ValueType = SettingValueType.SecureString,
                //    DefaultValue = "qwerty"
                //};

                public static IEnumerable<SettingDescriptor> AllSettings
                {
                    get
                    {
                        yield return VirtoCommerceCustomerSegmentsModuleEnabled;
                        //yield return VirtoCommerceCustomerSegmentsModulePassword;
                    }
                }
            }

            public static IEnumerable<SettingDescriptor> AllSettings
            {
                get
                {
                    return General.AllSettings;
                }
            }
        }
    }
}
