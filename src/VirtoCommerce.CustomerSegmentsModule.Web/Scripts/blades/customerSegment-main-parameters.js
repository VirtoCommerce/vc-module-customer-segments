angular.module('virtoCommerce.customerSegmentsModule')
    .controller('virtoCommerce.customerSegmentsModule.customerSegmentMainParametersController',
        ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
            var blade = $scope.blade;

            blade.refresh = () => {
                blade.currentEntity = angular.copy(blade.originalEntity);
                blade.isLoading = false;
            };

            var formScope;
            $scope.setForm = (form) => { formScope = form; };

            $scope.isValid = () => {
                return formScope && formScope.$valid;
            };

            // datepicker
            $scope.datepickers = {
                str: false,
                end: false
            };

            $scope.open = ($event, which) => {
                $event.preventDefault();
                $event.stopPropagation();
                $scope.datepickers[which] = true;
            };

            $scope.cancelChanges = () => {
                $scope.bladeClose();
            };

            $scope.saveChanges = () => {
                if (blade.currentEntity.isActive === undefined) {
                    blade.currentEntity.isActive = false;
                }

                if (blade.onSelected) {
                    blade.onSelected(blade.currentEntity);
                }

                $scope.bladeClose();
            };

            $scope.bladeClose = () => {
                blade.parentBlade.activeBladeId = null;
                bladeNavigationService.closeBlade(blade);
            };

            blade.refresh();
        }]);
