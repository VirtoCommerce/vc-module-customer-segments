using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using VirtoCommerce.CoreModule.Core.Conditions;
using VirtoCommerce.CustomerModule.Core.Model;
using VirtoCommerce.CustomerSegmentsModule.Core.Models;
using VirtoCommerce.CustomerSegmentsModule.Core.Models.Search;
using VirtoCommerce.CustomerSegmentsModule.Core.Services;
using VirtoCommerce.CustomerSegmentsModule.Data.Services;
using VirtoCommerce.Platform.Core.DynamicProperties;
using Xunit;

namespace VirtoCommerce.CustomerSegmentsModule.Tests
{
    public class UserGroupEvaluatorTests
    {
        [Fact]
        public async Task EvaluateUserGroupsAsync_HasStaticGroups_GroupsReturned()
        {
            // Arrange
            var staticGroups = new[] { "StaticUserGroup1", "StaticUserGroup2" };
            var evaluateContext = new UserGroupEvaluationContext()
            {
                Customer = new Contact
                {
                    Groups = staticGroups
                }
            };

            var searchResult = new CustomerSegmentSearchResult
            {
                Results = new List<CustomerSegment>()
                {
                    new CustomerSegment
                    {
                        ExpressionTree = new CustomerSegmentTree()
                    }
                }
            };

            var searchService = new Mock<ICustomerSegmentSearchService>();
            searchService.Setup(x => x.SearchCustomerSegmentsAsync(It.IsAny<CustomerSegmentSearchCriteria>())).ReturnsAsync(searchResult);
            var evaluator = new UserGroupEvaluator(searchService.Object);

            // Act
            var evaluationResult = await evaluator.EvaluateUserGroupsAsync(evaluateContext);

            // Assert
            var expectedResult = staticGroups.Select(group => new UserGroupInfo
            {
                UserGroup = group
            });

            evaluationResult.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task EvaluateUserGroupsAsync_HasDynamicGroups_GroupsReturned()
        {
            // Arrange
            var birthDate = DateTime.UtcNow;
            var organizations = new string[] { "Org1", "Org2" };
            var associatedOrganizations = new string[] { "Org3", "Org4" };
            var firstName = "Test first name";
            var middleName = "Test middle name";
            var lastName = "Test last name";
            var fullName = "Test full name";
            var salutation = "Test salutation";
            var defaultLanguage = "en-US";
            var timeZone = "UTC";
            var taxPayerId = "TesTaxPayerId";
            var preferredDelivery = "PreferredDelivery";
            var preferredCommunication = "PreferredCommunication";

            var dynamicPropertis = new DynamicObjectProperty[]
            {
                new DynamicObjectProperty
                {
                    Values = new DynamicPropertyObjectValue[]
                    {
                        new DynamicPropertyObjectValue
                        {
                            Value = "DynamicPropertyObjectValueTest",
                            ValueType = DynamicPropertyValueType.ShortText
                        },
                        new DynamicPropertyObjectValue
                        {
                            Value = "DynamicPropertyObjectValueTest2",
                            ValueType = DynamicPropertyValueType.ShortText
                        }
                    }
                }
            };

            var evaluateContext = new UserGroupEvaluationContext()
            {
                Customer = new Contact
                {
                    Groups = new List<string>(),
                    DynamicProperties = dynamicPropertis,
                    Organizations = organizations,
                    AssociatedOrganizations = associatedOrganizations,
                    BirthDate = birthDate,
                    FirstName = firstName,
                    MiddleName = middleName,
                    LastName = lastName,
                    FullName = fullName,
                    Salutation = salutation,
                    DefaultLanguage = defaultLanguage,
                    TimeZone = timeZone,
                    TaxPayerId = taxPayerId,
                    PreferredDelivery = preferredDelivery,
                    PreferredCommunication = preferredCommunication
                }
            };

            var expressionTree = new CustomerSegmentTree()
            {
                Children = new IConditionTree[]
                {
                    new CustomerSegmentConditionPropertyValues
                    {
                        Properties = dynamicPropertis,
                        Organizations = organizations,
                        AssociatedOrganizations = associatedOrganizations,
                        FirstName = firstName,
                        LastName = lastName,
                        FullName = fullName,
                        BirthDate = birthDate,
                    }
                }
            };

            var searchResult = new CustomerSegmentSearchResult
            {
                Results = new List<CustomerSegment>()
                {
                    new CustomerSegment
                    {
                        Id = "CustomerSegmentId",
                        Name = "Test Customer Segment Name",
                        UserGroup = "Dynamic User Group",
                        ExpressionTree = expressionTree
                    }
                }
            };

            var searchService = new Mock<ICustomerSegmentSearchService>();
            searchService.Setup(x => x.SearchCustomerSegmentsAsync(It.IsAny<CustomerSegmentSearchCriteria>())).ReturnsAsync(searchResult);
            var evaluator = new UserGroupEvaluator(searchService.Object);

            // Act
            var evaluationResult = await evaluator.EvaluateUserGroupsAsync(evaluateContext);

            // Assert
            var expectedResult = searchResult.Results.Select(s => new UserGroupInfo
            {
                IsDynamic = true,
                DynamicRuleId = s.Id,
                DynamicRuleName = s.Name,
                UserGroup = s.UserGroup
            });

            evaluationResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}
