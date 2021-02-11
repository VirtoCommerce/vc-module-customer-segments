angular.module('virtoCommerce.customerSegmentsModule')
    .controller('virtoCommerce.customerSegmentsModule.organizationsController', ['$scope', '$q', 'virtoCommerce.customerModule.members', 'virtoCommerce.customerModule.organizations',
        function ($scope, $q, members, organizations) {
            var blade = $scope.blade;

            $scope.pageSize = 50;

            $scope.fetchOrganizations = function ($select) {
                $select.page = 0;
                $scope.organizations = [];
                return $q.all([loadCustomerOrganizations(), $scope.fetchNextOrganizations($select)]);
            }

            $scope.fetchNextOrganizations = function ($select) {
                return members.search(
                    {
                        memberType: 'Organization',
                        SearchPhrase: $select.search,
                        deepSearch: true,
                        take: $scope.pageSize,
                        skip: $select.page * $scope.pageSize
                    },
                    function (data) {
                        joinOrganizations(data.results);
                        $select.page++;
                    }).$promise;
            };

            function loadCustomerOrganizations() {
                if (blade.currentEntity.organizations && blade.currentEntity.organizations.length > 0) {
                    return organizations.getByIds({ ids: blade.currentEntity.organizations }, function (data) {
                        joinOrganizations(data);
                    }
                    ).$promise;
                };
                return $q.resolve();
            };

            function joinOrganizations(organizations) {
                $scope.organizations = $scope.organizations.concat(organizations);
            };

            $scope.fetchCustomerOrganizations = function ($select) {
                $scope.fetchOrganizations($select).then(function () {
                    $scope.customerOrganizations = angular.copy($scope.organizations);
                });
            }

            $scope.fetchAssociatedOrganizations = function ($select) {
                $scope.fetchOrganizations($select).then(function () {
                    $scope.associatedOrganizations = angular.copy($scope.organizations);
                });
            }

            $scope.fetchNextCustomerOrganizations = function ($select) {
                $scope.fetchNextOrganizations($select).then(function () {
                    $scope.customerOrganizations = angular.copy($scope.organizations);
                });
            }

            $scope.fetchNextAssociatedOrganizations = function ($select) {
                $scope.fetchNextOrganizations($select).then(function () {
                    $scope.associatedOrganizations = angular.copy($scope.organizations);
                });
            }
        }]);
