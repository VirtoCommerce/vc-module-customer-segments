angular.module('virtoCommerce.customerSegmentsModule')
    .factory('virtoCommerce.customerSegmentsModule.expressionTreeHelper', function () {
        const expressionTreeBlockCustomerSegmentRuleId = "BlockCustomerSegmentRule";
        const expressionTreeConditionPropertyValuesId = "CustomerSegmentConditionPropertyValues";

        let getCustomerModelProperties = () => {
            var customerModelProperties = [
                {
                    name: 'salutation',
                    valueType: 'ShortText',
                    title: 'customer.blades.contact-detail.labels.salutation'
                },
                {
                    name: 'fullName',
                    valueType: 'ShortText',
                    title: 'customer.blades.contact-detail.labels.full-name'
                },
                {
                    name: 'firstName',
                    valueType: 'ShortText',
                    title: 'customer.blades.contact-detail.labels.first-name'
                },
                {
                    name: "middleName",
                    valueType: 'ShortText',
                    title: 'customerSegments.blades.customer-segment-properties.labels.middle-name'
                },
                {
                    name: 'lastName',
                    valueType: 'ShortText',
                    title: 'customer.blades.contact-detail.labels.last-name'
                },
                {
                    name: 'birthDate',
                    valueType: 'DateTime',
                    title: 'customer.blades.contact-detail.labels.birthday'
                },
                {
                    name: 'defaultLanguage',
                    valueType: 'ShortText',
                    title: 'customer.blades.contact-detail.labels.defaultLanguage'
                },
                {
                    name: 'timeZone',
                    valueType: 'ShortText',
                    title: 'customer.blades.contact-detail.labels.timezone'
                },
                {
                    name: 'taxPayerId',
                    valueType: 'ShortText',
                    title: 'customer.blades.contact-detail.labels.taxpayerId'
                },
                {
                    name: 'preferredDelivery',
                    valueType: 'ShortText',
                    title: 'customer.blades.contact-detail.labels.preferred-delivery'
                },
                {
                    name: 'preferredCommunication',
                    valueType: 'ShortText',
                    title: 'customer.blades.contact-detail.labels.preferred-communication'
                },
                {
                    name: 'organizations',
                    searchableName: 'parentorganizations',
                    templateUrl: 'organizationSelector.html',
                    isArray: true,
                    title: 'customer.blades.contact-detail.labels.organizations'
                },
                {
                    name: 'associatedOrganizations',
                    templateUrl: 'organizationSelector.html',
                    isArray: true,
                    title: 'customer.blades.contact-detail.labels.associated-organizations'
                }
            ];

            _.each(customerModelProperties, (property) => {
                property.isModelProperty = true;
                property.values = [];
            });

            return customerModelProperties;
        }

        return {
            getCustomerModelProperties: getCustomerModelProperties,
            extractSelectedProperties: (customerSegment) => {
                let result = [];

                const customerSegmentRuleBlock = customerSegment.expressionTree.children.find(x => x.id === expressionTreeBlockCustomerSegmentRuleId);

                if (!customerSegmentRuleBlock) {
                    throw new Error(expressionTreeBlockCustomerSegmentRuleId + " block is missing in expression tree");
                }

                if (customerSegmentRuleBlock.children[0]) {
                    result = customerSegmentRuleBlock.children[0].properties;

                    let modelProperties = customerSegment.customerModelProperties || getCustomerModelProperties();
                    const childen = customerSegmentRuleBlock.children[0];
                    for (const propertyName in childen) {
                        var modelProperty = _.findWhere(modelProperties, { name: propertyName });
                        if (modelProperty) {
                            if (modelProperty.isArray) {
                                modelProperty.values = childen[propertyName];
                            }
                            else {
                                modelProperty.values = [{ value: childen[propertyName] }];
                            }

                            if (_.some(modelProperty.values) && !_.some(result, (property) => {
                                return property.name === modelProperty.name;
                            })) {
                                result.push(modelProperty);
                            }
                        }
                    }
                }

                return result;
            },
            updateExpressionTree: (customerSegment, selectedProperties) => {
                let conditionPropertyValues = {
                    id: expressionTreeConditionPropertyValuesId,
                    properties: selectedProperties
                }

                // transform model properties
                _.each(conditionPropertyValues.properties, (property) => {
                    if (property.isModelProperty) {
                        if (property.isArray) {
                            conditionPropertyValues[property.name] = property.values;
                        }
                        else {
                            if (property.values && property.values.length) {
                                conditionPropertyValues[property.name] = property.values[0].value;
                            }
                        }
                    }
                });

                const customerSegmentRuleBlock = customerSegment.expressionTree.children.find(x => x.id === expressionTreeBlockCustomerSegmentRuleId);
                customerSegmentRuleBlock.children = [];

                customerSegmentRuleBlock.children.push(conditionPropertyValues);
            },
            transformResult: (customerSegment) => {
                const customerSegmentRuleBlock = customerSegment.expressionTree.children.find(x => x.id === expressionTreeBlockCustomerSegmentRuleId);

                if (customerSegmentRuleBlock.children[0]) {
                    customerSegmentRuleBlock.children[0].properties = _.filter(customerSegmentRuleBlock.children[0].properties, (property) => { return !property.isModelProperty; });
                }
            },
            updatePropertiesForPreview: (properties) => {
                _.each(properties, (property) => {
                    if (!property.isModelProperty && property.values && property.values.length) {
                        var values = _.map(property.values, function(value) {
                            return {
                                value: value.value.name,
                                valueId: value.value.id,
                            }
                        });
                        property.values = values;
                    }
                });
            }
        };
    });
