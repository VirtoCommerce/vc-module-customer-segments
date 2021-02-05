angular.module('virtoCommerce.customerSegmentsModule')
    .factory('virtoCommerce.customerSegmentsModule.webApi', ['$resource', function ($resource) {
        return $resource('api/VirtoCommerceCustomerSegmentsModule');
}]);
