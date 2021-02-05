angular.module('virtoCommerce.customerSegmentsModule')
    .controller('virtoCommerce.customerSegmentsModule.helloWorldController', ['$scope', 'virtoCommerce.customerSegmentsModule.webApi', function ($scope, api) {
        var blade = $scope.blade;
        blade.title = 'VirtoCommerce.CustomerSegmentsModule';

        blade.refresh = function () {
            api.get(function (data) {
                blade.title = 'virtoCommerce.customerSegmentsModule.blades.hello-world.title';
                blade.data = data.result;
                blade.isLoading = false;
            });
        };

        blade.refresh();
    }]);
