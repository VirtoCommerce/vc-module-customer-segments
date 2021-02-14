using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.CoreModule.Core.Conditions;
using VirtoCommerce.CustomerSegmentsModule.Core;
using VirtoCommerce.CustomerSegmentsModule.Core.Models;
using VirtoCommerce.CustomerSegmentsModule.Core.Services;
using VirtoCommerce.CustomerSegmentsModule.Data.Repositories;
using VirtoCommerce.CustomerSegmentsModule.Data.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;


namespace VirtoCommerce.CustomerSegmentsModule.Web
{
    public class Module : IModule
    {
        public ManifestModuleInfo ModuleInfo { get; set; }

        public void Initialize(IServiceCollection serviceCollection)
        {
            // database initialization
            serviceCollection.AddDbContext<CustomerSegmentDbContext>((provider, options) =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                options.UseSqlServer(configuration.GetConnectionString(ModuleInfo.Id) ?? configuration.GetConnectionString("VirtoCommerce"));
            });
            serviceCollection.AddTransient<ICustomerSegmentRepository, CustomerSegmentRepository>();
            serviceCollection.AddTransient<Func<ICustomerSegmentRepository>>(provider => () => provider.CreateScope().ServiceProvider.GetRequiredService<ICustomerSegmentRepository>());

            serviceCollection.AddTransient<ICustomerSegmentService, CustomerSegmentService>();
            serviceCollection.AddTransient<ICustomerSegmentSearchService, CustomerSegmentSearchService>();
            serviceCollection.AddTransient<IUserGroupEvaluator, UserGroupEvaluator>();
        }

        public void PostInitialize(IApplicationBuilder appBuilder)
        {
            AbstractTypeFactory<IConditionTree>.RegisterType<BlockCustomerSegmentRule>();
            AbstractTypeFactory<IConditionTree>.RegisterType<CustomerSegmentConditionPropertyValues>();

            // register settings
            var settingsRegistrar = appBuilder.ApplicationServices.GetRequiredService<ISettingsRegistrar>();
            settingsRegistrar.RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);

            // register permissions
            var permissionsProvider = appBuilder.ApplicationServices.GetRequiredService<IPermissionsRegistrar>();
            permissionsProvider.RegisterPermissions(ModuleConstants.Security.Permissions.AllPermissions.Select(x =>
                new Permission()
                {
                    GroupName = "Customer Segments",
                    ModuleId = ModuleInfo.Id,
                    Name = x
                }).ToArray());

            // Ensure that any pending migrations are applied
            using (var serviceScope = appBuilder.ApplicationServices.CreateScope())
            {
                using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<CustomerSegmentDbContext>())
                {
                    dbContext.Database.EnsureCreated();
                    dbContext.Database.Migrate();
                }
            }
        }

        public void Uninstall()
        {
            // do nothing in here
        }
    }
}
