angular.module('virtoCommerce.customerSegmentsModule')
    .controller('virtoCommerce.customerSegmentsModule.customerSegmentDetailController',
        ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.customerSegmentsModule.customerSegmentsApi', 'virtoCommerce.customerSegmentsModule.expressionTreeHelper', 'virtoCommerce.customerSegmentsModule.customerHelper',
            function ($scope, bladeNavigationService, customerSegmentsApi, expressionTreeHelper, customerHelper) {
                const blade = $scope.blade;
                blade.currentEntity = {};
                blade.customersCount = 0;

                blade.refresh = () => {
                    blade.isLoading = true;
                    if (blade.isNew) {
                        customerSegmentsApi.new({},
                            data => {
                                blade.originalEntity = data;
                                blade.currentEntity = angular.copy(blade.originalEntity);
                                blade.isLoading = false;
                            }, err => {
                                $scope.bladeClose();
                                bladeNavigationService.setError(err, blade.parentBlade);
                                blade.parentBlade.$scope.showErrorDetails();
                            });
                    } else {
                        blade.currentEntity = angular.copy(blade.originalEntity);
                        blade.mainParametersAreSet = true;
                        blade.ruleIsSet = true;
                        refreshCustomersCount();
                        blade.originalEntity = angular.copy(blade.currentEntity);
                        blade.isLoading = false;
                    }
                }

                function refreshCustomersCount() {
                    var customerSegment = angular.copy(blade.currentEntity);
                    const selectedProperties = expressionTreeHelper.extractSelectedProperties(customerSegment);

                    if (_.any(selectedProperties)) {
                        customerHelper.getCustomersCount(customerSegment, selectedProperties).then((x) => blade.customersCount = x);
                    } else {
                        blade.customersCount = 0;
                    }
                }

                blade.onClose = (closeCallback) => {
                    bladeNavigationService.showConfirmationIfNeeded(isDirty() && !blade.isNew, $scope.isValid(), blade, $scope.saveChanges, closeCallback, "customerSegments.dialogs.customer-segment-save.title", "customerSegments.dialogs.customer-segment-save.message");
                };

                var formScope;
                $scope.setForm = (form) => { formScope = form; };

                $scope.isValid = () => {
                    return formScope && formScope.$valid;
                };

                $scope.canSave = () => {
                    return isDirty() && $scope.isValid() && blade.mainParametersAreSet && blade.ruleIsSet;
                };

                function isDirty() {
                    return !angular.equals(blade.currentEntity, blade.originalEntity);
                }

                $scope.saveChanges = () => {
                    expressionTreeHelper.transformResult(blade.currentEntity);

                    customerSegmentsApi.save({}, blade.currentEntity, () => {
                        blade.parentBlade.refresh();
                        blade.originalEntity = blade.currentEntity;
                        $scope.closeBlade();
                    }, () => {
                        $scope.showErrorDetails();
                    });
                };

                $scope.mainParameters = () => {
                    const parametersBlade = {
                        id: "mainParameters",
                        title: "customerSegments.blades.customer-segment-parameters.title",
                        subtitle: 'customerSegments.blades.customer-segment-parameters.subtitle',
                        controller: 'virtoCommerce.customerSegmentsModule.customerSegmentMainParametersController',
                        template: 'Modules/$(VirtoCommerce.CustomerSegments)/Scripts/blades/customerSegment-main-parameters.tpl.html',
                        originalEntity: blade.currentEntity,
                        onSelected: (entity) => {
                            blade.currentEntity = entity;
                            blade.mainParametersAreSet = true;
                        }
                    };
                    blade.activeBladeId = parametersBlade.id;
                    bladeNavigationService.showBlade(parametersBlade, blade);
                };

                $scope.createCustomerFilter = () => {
                    var ruleCreationBlade = {
                        id: "createCustomerSegmentRule",
                        controller: 'virtoCommerce.customerSegmentsModule.customerSegmentRuleController',
                        title: 'customerSegments.blades.customer-segment-rule-creation.title',
                        subtitle: 'customerSegments.blades.customer-segment-rule-creation.subtitle',
                        template: 'Modules/$(VirtoCommerce.CustomerSegments)/Scripts/blades/customerSegment-rule.tpl.html',
                        originalEntity: blade.currentEntity,
                        onSelected: (entity) => {
                            blade.currentEntity = entity;
                            blade.ruleIsSet = true;
                            refreshCustomersCount();
                        }
                    };
                    blade.activeBladeId = ruleCreationBlade.id;
                    bladeNavigationService.showBlade(ruleCreationBlade, blade);
                };

                $scope.$watch('blade.currentEntity', (data) => {
                    if (data) {
                        $scope.totalPropertiesCount = 3;
                        $scope.filledPropertiesCount = (blade.currentEntity.isActive !== undefined ? 1 : 0)
                            + (blade.currentEntity.startDate ? 1 : 0)
                            + (blade.currentEntity.endDate ? 1 : 0);
                    }
                }, true);

                $scope.closeBlade = () => {
                    bladeNavigationService.closeBlade(blade);
                };

                blade.refresh();
            }]);
