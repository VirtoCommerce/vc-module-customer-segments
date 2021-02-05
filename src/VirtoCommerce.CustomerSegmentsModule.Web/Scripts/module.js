// Call this to register your module to main application
var moduleName = "virtoCommerce.customerSegmentsModule";

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .config(['$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {
            $stateProvider
                .state('workspace.virtoCommerceCustomerSegmentsModuleState', {
                    url: '/virtoCommerce.customerSegmentsModule',
                    templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
                    controller: [
                        '$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                            var newBlade = {
                                id: 'blade1',
                                controller: 'virtoCommerce.customerSegmentsModule.helloWorldController',
                                template: 'Modules/$(VirtoCommerce.CustomerSegments)/Scripts/blades/hello-world.html',
                                isClosingDisabled: true
                            };
                            bladeNavigationService.showBlade(newBlade);
                        }
                    ]
                });
        }
    ])
    .run(['platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state',
        function (mainMenuService, widgetService, $state) {
            //Register module in main menu
            var menuItem = {
                path: 'browse/virtoCommerce.customerSegmentsModule',
                icon: 'fa fa-cube',
                title: 'VirtoCommerce.CustomerSegmentsModule',
                priority: 100,
                action: function () { $state.go('workspace.virtoCommerceCustomerSegmentsModuleState'); },
                permission: 'customerSegments:access'
            };
            mainMenuService.addMenuItem(menuItem);
        }
    ]);
