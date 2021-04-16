angular.module('virtoCommerce.customerSegmentsModule')
    .factory('virtoCommerce.customerSegmentsModule.customerSegmentsApi', ['$resource', function ($resource) {
        return $resource('api/customersegments/:id', {}, {
            new: { method: 'GET', url: 'api/customersegments/new' },
            save: { method: 'POST' },
            search: { method: 'POST', url: 'api/customersegments/search' },
            preview: { method: 'POST', url: 'api/customersegments/preview' }
        });
    }]);
