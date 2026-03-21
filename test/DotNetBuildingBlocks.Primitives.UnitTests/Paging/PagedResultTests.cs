using DotNetBuildingBlocks.Primitives.Paging;
using FluentAssertions;
using Xunit;

namespace DotNetBuildingBlocks.Primitives.UnitTests.Paging;

public sealed class PagedResultTests
{
    private static readonly int[] ThreeItems = [1, 2, 3];
    private static readonly int[] OneItem = [1];

    [Fact]
    public void TotalPages_Should_Be_Calculated_Correctly()
    {
        var result = new PagedResult<int>(ThreeItems, 2, 10, 25);

        result.TotalPages.Should().Be(3);
    }

    [Fact]
    public void HasPreviousPage_Should_Be_True_When_PageNumber_Greater_Than_One()
    {
        var result = new PagedResult<int>(OneItem, 2, 10, 25);

        result.HasPreviousPage.Should().BeTrue();
    }

    [Fact]
    public void HasNextPage_Should_Be_True_When_PageNumber_Is_Less_Than_TotalPages()
    {
        var result = new PagedResult<int>(OneItem, 2, 10, 25);

        result.HasNextPage.Should().BeTrue();
    }

    [Fact]
    public void Empty_Should_Return_Empty_Result()
    {
        var result = PagedResult<int>.Empty(1, 20);

        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
        result.TotalPages.Should().Be(0);
        result.HasPreviousPage.Should().BeFalse();
        result.HasNextPage.Should().BeFalse();
    }
}
