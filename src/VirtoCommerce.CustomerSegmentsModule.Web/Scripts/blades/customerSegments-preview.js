angular.module('virtoCommerce.customerSegmentsModule')
    .controller('virtoCommerce.customerSegmentsModule.customerSegmentsPreview',
        ['$scope',
            'platformWebApp.bladeUtils', 'platformWebApp.uiGridHelper', 'platformWebApp.ui-grid.extension',
            'virtoCommerce.customerModule.memberTypesResolverService', 'virtoCommerce.customerSegmentsModule.customerSegmentsApi', 'virtoCommerce.customerSegmentsModule.expressionTreeHelper',
            function ($scope, bladeUtils, uiGridHelper, gridOptionExtension, memberTypesResolverService, customerSegments, expressionTreeHelper) {
                $scope.uiGridConstants = uiGridHelper.uiGridConstants;

                var blade = $scope.blade;
                blade.headIcon = 'fa fa-eye';
                blade.title = 'customerSegments.blades.customer-segment-preview.title';
                blade.toolbarCommands = [
                    {
                        name: "platform.commands.refresh", icon: 'fa fa-refresh',
                        executeMethod: () => blade.refresh(),
                        canExecuteMethod: () => true
                    }
                ];

                blade.refresh = () => {
                    blade.isLoading = true;

                    expressionTreeHelper.updatePropertiesForPreview(blade.properties);
                    expressionTreeHelper.updateExpressionTree(blade.customerSegment, blade.properties);
                    expressionTreeHelper.transformResult(blade.customerSegment);

                    var request = getSearchCriteria(blade.customerSegment.expressionTree);
                    customerSegments.preview(request, function(searchResult) {
                        $scope.pageSettings.totalItems = searchResult.totalCount;
                        let memberTypeDefinition;
                        _.each(searchResult.results, (x) => {
                            if ((memberTypeDefinition = memberTypesResolverService.resolve(x.memberType)) !== null) {
                                x._memberTypeIcon = memberTypeDefinition.icon;
                            }
                        });
                        $scope.customers = searchResult.results || [];
                        blade.isLoading = false;
                    });
                }

                var filter = $scope.filter = {};

                filter.criteriaChanged = () => {
                    if ($scope.pageSettings.currentPage > 1) {
                        $scope.pageSettings.currentPage = 1;
                    } else {
                        blade.refresh();
                    }
                };

                // ui-grid
                $scope.setGridOptions = (gridId, gridOptions) => {
                    $scope.gridOptions = gridOptions;
                    gridOptionExtension.tryExtendGridOptions(gridId, gridOptions);

                    gridOptions.onRegisterApi = (gridApi) => {
                        $scope.gridApi = gridApi;
                        gridApi.core.on.sortChanged($scope, () => {
                            if (!blade.isLoading) blade.refresh();
                        });
                    };

                    bladeUtils.initializePagination($scope);
                };

                function getSearchCriteria(expression) {
                    let searchCriteria = {
                        searchPhrase: filter.keyword,
                        expression: expression,
                        sort: uiGridHelper.getSortExpression($scope),
                        skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                        take: $scope.pageSettings.itemsPerPageCount
                    }
                    return searchCriteria;
                }
            }]);
