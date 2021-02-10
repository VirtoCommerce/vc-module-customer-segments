angular.module('virtoCommerce.customerSegmentsModule')
    .factory('virtoCommerce.customerSegmentsModule.customerHelper', ['$q', 'virtoCommerce.customerSegmentsModule.customerSearchCriteriaBuilder', 'virtoCommerce.customerModule.members',
        function ($q, customerSearchCriteriaBuilder, membersApi) {
            return {
                getCustomersCount: (keyword, properties) => {
                    var deferred = $q.defer();
                    let searchCriteria = customerSearchCriteriaBuilder.build(keyword, properties);
                    searchCriteria.skip = 0;
                    searchCriteria.take = 0;
                    membersApi.search(searchCriteria, searchResult => {
                        deferred.resolve(searchResult.totalCount);
                    });

                    return deferred.promise;
                }
            }
        }]);
