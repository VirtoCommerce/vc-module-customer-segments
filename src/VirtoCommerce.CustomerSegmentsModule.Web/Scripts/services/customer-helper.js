angular.module('virtoCommerce.customerSegmentsModule')
    .factory('virtoCommerce.customerSegmentsModule.customerHelper', ['$q', 'virtoCommerce.customerSegmentsModule.customerSegmentsApi', 'virtoCommerce.customerSegmentsModule.expressionTreeHelper',
        function ($q, customerSegments, expressionTreeHelper) {
            return {
                getCustomersCount: (customerSegment, properties) => {
                    var deferred = $q.defer();

                    expressionTreeHelper.updatePropertiesForPreview(properties);
                    expressionTreeHelper.updateExpressionTree(customerSegment, properties);
                    expressionTreeHelper.transformResult(customerSegment);

                    let request = {
                        expression: customerSegment.expressionTree,
                        skip: 0,
                        take: 0
                    };

                    customerSegments.preview(request, searchResult => {
                        deferred.resolve(searchResult.totalCount);
                    });

                    return deferred.promise;
                }
            }
        }]);
