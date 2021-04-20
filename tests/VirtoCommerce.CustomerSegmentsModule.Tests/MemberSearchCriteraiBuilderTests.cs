using FluentAssertions;
using VirtoCommerce.CustomerSegmentsModule.Core.Models;
using Xunit;

namespace VirtoCommerce.CustomerSegmentsModule.Tests
{
    public class MemberSearchCriteraiBuilderTests
    {
        [Fact]
        public void MemberSearchCriteriaBuilder_WithPagingTest()
        {
            var builder = new MemberSearchCriteraiBuilder();

            builder.WithPaging(20, 40);

            var criteria = builder.Build();

            criteria.Skip.Should().Be(20);
            criteria.Take.Should().Be(40);
        }

        [Fact]
        public void MemberSearchCriteriaBuilder_WithSortTest()
        {
            var builder = new MemberSearchCriteraiBuilder();

            builder.WithSort("name:desc");

            var criteria = builder.Build();

            criteria.Sort.Should().Be("name:desc");
        }

        [Fact]
        public void MemberSearchCriteriaBuilder_WithSearchPhraseTest()
        {
            var builder = new MemberSearchCriteraiBuilder();

            builder.WithSearchPhrase("test");
            builder.WithSearchPhrase("propertyName:propertyValue");

            var criteria = builder.Build();

            criteria.SearchPhrase.Should().Be("test propertyName:propertyValue");
        }
    }
}
