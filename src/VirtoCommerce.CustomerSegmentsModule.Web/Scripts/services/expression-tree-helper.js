angular.module('virtoCommerce.customerSegmentsModule')
    .factory('virtoCommerce.customerSegmentsModule.expressionTreeHelper', function () {
        const expressionTreeBlockCustomerSegmentRuleId = "BlockCustomerSegmentRule";
        const expressionTreeConditionPropertyValuesId = "ConditionPropertyValues";

        return {
            extractSelectedProperties: (customerSegment) => {
                let result = [];

                const customerSegmentRuleBlock = customerSegment.expressionTree.children.find(x => x.id === expressionTreeBlockCustomerSegmentRuleId);

                if (!customerSegmentRuleBlock) {
                    throw new Error(expressionTreeBlockCustomerSegmentRuleId + " block is missing in expression tree");
                }

                if (customerSegmentRuleBlock.children[0]) {
                    result = customerSegmentRuleBlock.children[0].properties;

                    const childen = customerSegmentRuleBlock.children[0];
                    for (const propertyName in childen) {
                        var modelProperty = _.find(customerSegment.customerModelProperties, (property) => { return property.name === propertyName; });
                        if (modelProperty) {
                            //modelProperty = angular.copy(modelProperty);

                            if (modelProperty.isArray) {

                            }
                            else {
                                modelProperty.values = [{ value: childen[propertyName] }]
                            }

                            if (!_.some(result, (property) => {
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
            }
        };
    });
