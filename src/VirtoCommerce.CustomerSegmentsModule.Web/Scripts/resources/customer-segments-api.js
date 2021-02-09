angular.module('virtoCommerce.customerSegmentsModule')
    .factory('virtoCommerce.customerSegmentsModule.customerSegmentsApi', ['$resource', function ($resource) {
        //return $resource('api/demo/customersegments/:id', {}, {
        return $resource('api/customersegments/:id', {}, {
            new: { method: 'GET', url: 'api/customersegments/new' },
            save: { method: 'POST', isArray: true },
            //delete: { method: 'DELETE' },
            search: { method: 'POST', url: 'api/customersegments/search' }
        });
    }]);
