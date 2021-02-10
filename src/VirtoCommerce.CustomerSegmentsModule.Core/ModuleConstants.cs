using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.CustomerSegmentsModule.Core
{
    public static class ModuleConstants
    {
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
                public static SettingDescriptor MaxAllowedSegments { get; } = new SettingDescriptor
                {
                    Name = "CustomerSegments.General.MaxAllowedSegments",
                    GroupName = "Customer Segments|General",
                    ValueType = SettingValueType.Integer,
                    DefaultValue = 1000
                };

                public static SettingDescriptor MaxActiveSegments { get; } = new SettingDescriptor
                {
                    Name = "CustomerSegments.General.MaxActiveSegments",
                    GroupName = "Customer Segments|General",
                    ValueType = SettingValueType.Integer,
                    DefaultValue = 20
                };

                public static IEnumerable<SettingDescriptor> AllSettings
                {
                    get
                    {
                        yield return MaxAllowedSegments;
                        yield return MaxActiveSegments;
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
