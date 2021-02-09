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
                }

                return result;
            },
            updateExpressionTree: (customerSegment, selectedProperties) => {
                const customerSegmentRuleBlock = customerSegment.expressionTree.children.find(x => x.id === expressionTreeBlockCustomerSegmentRuleId);
                customerSegmentRuleBlock.children = [];

                customerSegmentRuleBlock.children.push({
                    id: expressionTreeConditionPropertyValuesId,
                    properties: selectedProperties
                });
            }
        };
    });
