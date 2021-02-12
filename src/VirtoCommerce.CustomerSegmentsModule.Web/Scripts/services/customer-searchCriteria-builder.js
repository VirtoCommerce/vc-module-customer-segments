angular.module('virtoCommerce.customerSegmentsModule')
    .factory('virtoCommerce.customerSegmentsModule.customerSearchCriteriaBuilder', function () {
        return {
            build: (keyword, properties) => {
                let searchPhrase = [];
                if (keyword) {
                    searchPhrase.push(keyword);
                }

                function convertValue(value) {
                    var result = value;

                    if (value instanceof Date) {
                        result = value.toISOString();
                    }

                    return result;
                }

                if (properties) {
                    properties.forEach(property => {
                        let valuesMap = [];

                        if (property.isModelProperty && property.isArray && _.some(property.values)) {
                            valuesMap = property.values.map(value => convertValue(value));
                        }
                        else {
                            valuesMap = property.values.map(value => value.value !== undefined && value.value !== null ? value.value.name || convertValue(value.value) : '');
                        }

                        const values = valuesMap.join('","');
                        searchPhrase.push(`"${property.searchableName || property.name}":"${values}"`);
                    });
                }

                return {
                    searchPhrase: searchPhrase.join(' '),
                    deepSearch: true,
                    responseGroup: 'default',
                    memberType: 'Contact'
                };
            }
        }
    });
