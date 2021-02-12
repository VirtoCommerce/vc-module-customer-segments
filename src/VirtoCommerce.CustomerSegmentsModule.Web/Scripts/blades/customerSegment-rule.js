angular.module('virtoCommerce.customerSegmentsModule')
    .controller('virtoCommerce.customerSegmentsModule.customerSegmentRuleController',
        ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dynamicProperties.api', 'virtoCommerce.customerSegmentsModule.expressionTreeHelper', 'virtoCommerce.customerSegmentsModule.customerHelper',
            function ($scope, bladeNavigationService, dynamicPropertiesApi, expressionTreeHelper, customerHelper) {
                var blade = $scope.blade;
                blade.activeBladeId = null;
                blade.propertiesCount = 0;
                blade.selectedPropertiesCount = 0;
                blade.customersCount = 0;

                var properties = [];
                var customerModelProperties = [
                    {
                        name: 'salutation',
                        valueType: 'ShortText',
                        title: 'customer.blades.contact-detail.labels.salutation'
                    },
                    {
                        name: 'fullName',
                        valueType: 'ShortText',
                        title: 'customer.blades.contact-detail.labels.full-name'
                    },
                    {
                        name: 'firstName',
                        valueType: 'ShortText',
                        title: 'customer.blades.contact-detail.labels.first-name'
                    },
                    {
                        name: "middleName",
                        valueType: 'ShortText',
                        title: 'customerSegments.blades.customer-segment-properties.labels.middle-name'
                    },
                    {
                        name: 'lastName',
                        valueType: 'ShortText',
                        title: 'customer.blades.contact-detail.labels.last-name'
                    },
                    {
                        name: 'birthDate',
                        valueType: 'DateTime',
                        title: 'customer.blades.contact-detail.labels.birthday'
                    },
                    {
                        name: 'defaultLanguage',
                        valueType: 'ShortText',
                        title: 'customer.blades.contact-detail.labels.defaultLanguage'
                    },
                    {
                        name: 'timeZone',
                        valueType: 'ShortText',
                        title: 'customer.blades.contact-detail.labels.timezone'
                    },
                    {
                        name: 'taxPayerId',
                        valueType: 'ShortText',
                        title: 'customer.blades.contact-detail.labels.taxpayerId'
                    },
                    {
                        name: 'preferredDelivery',
                        valueType: 'ShortText',
                        title: 'customer.blades.contact-detail.labels.preferred-delivery'
                    },
                    {
                        name: 'preferredCommunication',
                        valueType: 'ShortText',
                        title: 'customer.blades.contact-detail.labels.preferred-communication'
                    },
                    {
                        name: 'organizations',
                        searchableName: 'parentorganizations',
                        templateUrl: 'organizationSelector.html',
                        isArray: true,
                        title: 'customer.blades.contact-detail.labels.organizations'
                    },
                    {
                        name: 'associatedOrganizations',
                        templateUrl: 'organizationSelector.html',
                        isArray: true,
                        title: 'customer.blades.contact-detail.labels.associated-organizations'
                    }
                ]

                function initializeBlade() {
                    _.each(customerModelProperties, (property) => {
                        property.isModelProperty = true;
                        property.values = [];
                    });

                    dynamicPropertiesApi.search({
                        "objectType": 'VirtoCommerce.CustomerModule.Core.Model.Contact',
                        "take": 100
                    },
                        response => {
                            _.each(response.results,
                                (property) => {
                                    property.isRequired = true;
                                    property.values = property.valueType === 'Boolean' ? [{ value: false }] : [];
                                });

                            properties = _.union(response.results, customerModelProperties);
                            blade.propertiesCount = response.totalCount + customerModelProperties.length;
                            blade.isLoading = false;
                        });

                    blade.currentEntity = angular.copy(blade.originalEntity);

                    blade.currentEntity.customerModelProperties = customerModelProperties;
                    blade.originalProperties = expressionTreeHelper.extractSelectedProperties(blade.currentEntity);

                    blade.selectedPropertiesCount = blade.originalProperties.length;

                    blade.selectedProperties = angular.copy(blade.originalProperties);

                    if (blade.selectedProperties && blade.selectedProperties.length > 0) {
                        refreshCustomersCount();
                    }
                }

                function isDirty() {
                    return !angular.equals(blade.selectedProperties, blade.originalProperties);
                }

                $scope.selectProperties = () => {
                    var newBlade = {
                        id: 'propertiesSelector',
                        title: 'customerSegments.blades.customer-segment-properties.title',
                        controller: 'virtoCommerce.customerSegmentsModule.customerSegmentPropertiesController',
                        template: 'Modules/$(VirtoCommerce.CustomerSegments)/Scripts/blades/customerSegment-properties.tpl.html',
                        originalEntity: blade.currentEntity,
                        properties: properties,
                        selectedProperties: blade.selectedProperties,
                        onSelected: (entity, selectedProperties) => {
                            blade.currentEntity = entity;
                            blade.selectedProperties = selectedProperties;
                            blade.selectedPropertiesCount = blade.selectedProperties.length;
                            $scope.editProperties();
                        }
                    };
                    blade.activeBladeId = newBlade.id;
                    bladeNavigationService.showBlade(newBlade, blade);
                };

                $scope.editProperties = () => {
                    var newBlade = {
                        id: 'propertiesEditor',
                        title: 'customerSegments.blades.customer-segment-property-values.title',
                        controller: 'virtoCommerce.customerSegmentsModule.customerSegmentPropertyValuesController',
                        template: 'Modules/$(VirtoCommerce.CustomerSegments)/Scripts/blades/customerSegment-property-values.tpl.html',
                        originalEntity: blade.currentEntity,
                        selectedProperties: blade.selectedProperties,
                        onSelected: (entity, selectedProperties) => {
                            blade.currentEntity = entity;
                            blade.selectedProperties = selectedProperties;
                            refreshCustomersCount();
                        }
                    };
                    blade.activeBladeId = newBlade.id;
                    bladeNavigationService.showBlade(newBlade, blade);
                };

                function refreshCustomersCount() {
                    customerHelper.getCustomersCount('', blade.selectedProperties).then((x) => blade.customersCount = x);
                }

                $scope.canSave = () => {
                    return isDirty() && blade.selectedProperties && blade.selectedProperties.length && blade.selectedProperties.every(x => x.values && x.values.length);
                };

                $scope.saveChanges = () => {
                    if (blade.onSelected) {
                        expressionTreeHelper.updateExpressionTree(blade.currentEntity, blade.selectedProperties);
                        blade.onSelected(blade.currentEntity);
                    }

                    $scope.bladeClose();
                };

                $scope.bladeClose = () => {
                    blade.parentBlade.activeBladeId = null;
                    bladeNavigationService.closeBlade(blade);
                };

                initializeBlade();
            }]);
