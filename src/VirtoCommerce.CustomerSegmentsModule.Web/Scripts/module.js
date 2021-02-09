// Call this to register your module to main application
var moduleName = "virtoCommerce.customerSegmentsModule";

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .run(['virtoCommerce.marketingModule.marketingMenuItemService',
        function (marketingMenuItemService) {
            marketingMenuItemService.register({
                id: 'customerSegments',
                name: 'Customer segments',
                entityName: 'customerSegment',
                icon: 'fa fa-pie-chart',
                controller: 'virtoCommerce.customerSegmentsModule.customerSegmentListController',
                template: 'Modules/$(VirtoCommerce.CustomerSegments)/Scripts/blades/customerSegment-list.tpl.html',
                // permission: 'customerSegments:access'
            });
        }
    ]);
