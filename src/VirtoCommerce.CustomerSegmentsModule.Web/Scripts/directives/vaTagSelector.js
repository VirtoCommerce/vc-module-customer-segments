angular.module('virtoCommerce.customerSegmentsModule')
    .directive('vaTagSelector', ['$q', 'platformWebApp.bladeNavigationService', 'platformWebApp.settings', function ($q, bladeNavigationService, settings) {
        const defaultPageSize = 5; // 50
        return {
            restrict: 'E',
            require: 'ngModel',
            replace: true,
            scope: {
                blade: '=',
                allowClear: '=?',
                disabled: '=?',
                multiple: '=?',
                pageSize: '=?',
                label: '=?',
                placeholder: '=?',
                required: '=?'
            },
            templateUrl: 'Modules/$(VirtoCommerce.CustomerSegments)/Scripts/directives/vaTagSelector.tpl.html',
            link: function ($scope, element, attrs, ngModelController) {
                $scope.context = {
                    modelValue: null,
                    allowClear: angular.isDefined(attrs.allowClear) && (attrs.allowClear === '' || attrs.allowClear.toLowerCase() === 'true'),
                    multiple: angular.isDefined(attrs.multiple) && (attrs.multiple === '' || attrs.multiple.toLowerCase() === 'true'),
                    required: angular.isDefined(attrs.required) && (attrs.required === '' || attrs.required.toLowerCase() === 'true')
                };

                // PageSize amount must be enough to show scrollbar in dropdown list container.
                // If scrollbar doesn't appear auto loading won't work.
                var pageSize = $scope.pageSize || defaultPageSize;

                $scope.choices = [];
                $scope.isNoChoices = true;
                var lastSearchPhrase = '';
                var totalCount = 0;

                $scope.fetch = function ($select) {
                    //loadEntityItems();
                    if (!$scope.disabled) {
                        $scope.fetchNext($select);
                    }
                };

                //function loadEntityItems() {
                //    var ids = $scope.context.multiple ? $scope.context.modelValue : [$scope.context.modelValue];

                //    if ($scope.isNoChoices && _.any(ids)) {
                //        settings.search({
                //            entityIds: ids,
                //            take: ids.length,
                //            responseGroup: 'none'
                //        }, (data) => {
                //            joinItems(data.results);
                //        });
                //    }
                //}

                $scope.fetchNext = ($select) => {
                    $select.page = $select.page || 0;

                    //if (lastSearchPhrase !== $select.search && totalCount > $scope.choices.length) {
                    //    lastSearchPhrase = $select.search;
                    //    $select.page = 0;
                    //}

                    if ($select.page === 0 || totalCount > $scope.choices.length) {
                        return settings.getValues(
                            {
                                id: 'Customer.MemberGroups'
                                //searchPhrase: $select.search,
                                //take: pageSize,
                                //skip: $select.page * pageSize,
                                //responseGroup: 'none'
                            }, (data) => {
                                joinItems(data);
                                $select.page++;

                                if ($select.page * pageSize < data.totalCount) {
                                    $scope.$broadcast('scrollCompleted');
                                }

                                totalCount = Math.max(totalCount, data.totalCount);
                            }).$promise;
                    }

                    return $q.resolve();
                };

                function joinItems(newItems) {
                    newItems = _.reject(newItems, x => _.any($scope.choices, y => y === x));
                    if (_.any(newItems)) {
                        $scope.choices = $scope.choices.concat(newItems);
                        $scope.isNoChoices = $scope.choices.length === 0;
                    }
                }

                $scope.$watch('context.modelValue', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        ngModelController.$setViewValue($scope.context.modelValue);
                    }
                });

                ngModelController.$render = function () {
                    $scope.context.modelValue = ngModelController.$modelValue;
                };

                $scope.openDictionarySettingManagement = () => {
                    var newBlade = {
                        id: 'settingDetailChild',
                        isApiSave: true,
                        currentEntityId: 'Customer.MemberGroups',
                        parentRefresh: (data) => {
                            $scope.choices = data;
                            $scope.isNoChoices = $scope.choices.length === 0;
                        },
                        controller: 'platformWebApp.settingDictionaryController',
                        template: '$(Platform)/Scripts/app/settings/blades/setting-dictionary.tpl.html'
                    };
                    bladeNavigationService.showBlade(newBlade, $scope.blade);
                };
            }
        }
    }]);
