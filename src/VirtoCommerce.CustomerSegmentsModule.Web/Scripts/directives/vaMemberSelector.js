angular.module('virtoCommerce.customerSegmentsModule')
    .directive('vaMemberSelector', ['virtoCommerce.customerModule.members', function (members) {
        const defaultPageSize = 25;
        return {
            restrict: 'E',
            require: 'ngModel',
            replace: true,
            scope: {
                disabled: '=?',
                memberTypes: '=?',
                multiple: '=?',
                pageSize: '=?',
                placeholder: '=?',
                required: '=?'
            },
            templateUrl: 'Modules/$(VirtoCommerce.CustomerSegments)/Scripts/directives/vaMemberSelector.tpl.html',
            link: function ($scope, element, attrs, ngModelController) {
                $scope.context = {
                    modelValue: null,
                    multiple: angular.isDefined(attrs.multiple) && (attrs.multiple === '' || attrs.multiple.toLowerCase() === 'true'),
                    required: angular.isDefined(attrs.required) && (attrs.required === '' || attrs.required.toLowerCase() === 'true')
                };


                // PageSize amount must be enough to show scrollbar in dropdown list container.
                // If scrollbar doesn't appear auto loading won't work.
                var pageSize = $scope.pageSize || defaultPageSize;

                $scope.choices = [];
                $scope.isNoChoices = true;
                var totalCount = 0;
                var memberTypes;

                $scope.fetch = function ($select) {
                    var ids = $scope.context.multiple ? $scope.context.modelValue : [$scope.context.modelValue];
                    memberTypes = memberTypes || (angular.isArray($scope.memberTypes) && _.some($scope.memberTypes) ? $scope.memberTypes : []);

                    if ($scope.isNoChoices && _.any(ids)) {
                        members.search(
                            {
                                objectIds: ids,
                                take: ids.length,
                                deepSearch: true,
                                responseGroup: 'default',
                                memberTypes: memberTypes
                            }, data => {
                            joinItems(data);
                        });
                    }

                    if (!$scope.disabled) {
                        $scope.fetchNext($select);
                    }
                };

                $scope.fetchNext = ($select) => {
                    $select.page = $select.page || 0;

                    if ($select.page === 0 || totalCount > $scope.choices.length) {
                        members.search(
                            {
                                searchPhrase: $select.search,
                                deepSearch: true,
                                take: pageSize,
                                skip: $select.page * pageSize,
                                responseGroup: 'default',
                                memberTypes: memberTypes
                            }, data => {
                                joinItems(data);
                                $select.page++;

                                if ($select.page * pageSize < data.totalCount) {
                                    $scope.$broadcast('scrollCompleted');
                                }

                                totalCount = Math.max(totalCount, data.totalCount);
                            });
                    }
                };

                function joinItems(newItems) {
                    newItems = _.reject(newItems.results, x => _.any($scope.choices, y => y === x));
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
            }
        }
    }]);
