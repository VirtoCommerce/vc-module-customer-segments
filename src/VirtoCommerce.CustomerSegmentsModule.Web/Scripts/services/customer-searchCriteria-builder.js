angular.module('virtoCommerce.customerSegmentsModule')
    .factory('virtoCommerce.customerSegmentsModule.customerSearchCriteriaBuilder', function () {
        return {
            build: (keyword, properties) => {
                let searchPhrase = [];
                if (keyword) {
                    searchPhrase.push(keyword);
                }

                if (properties) {
                    properties.forEach(property => {
                        if (property.name === 'organizations') {
                            if (_.some(property.values)) {
                                searchPhrase.push(`"parentorganizations":"${property.values.join('","')}"`);
                            }
                        } else if (property.name === 'associatedOrganizations') {
                            if (_.some(property.values)) {
                                 searchPhrase.push(`"associatedOrganizations":"${property.values.join('","')}"`);
                            }
                        } else {
                            const values = property.values.map(value => value.value !== undefined && value.value !== null ? value.value.name || value.value : '').join('","');
                            searchPhrase.push(`"${property.name}":"${values}"`);
                        }
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
