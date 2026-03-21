using DotNetBuildingBlocks.Primitives.Paging;
using FluentAssertions;
using Xunit;

namespace DotNetBuildingBlocks.Primitives.UnitTests.Paging;

public sealed class PagedRequestTests
{
    [Fact]
    public void Skip_Should_Be_Calculated_From_Page_And_Size()
    {
        var request = new PagedRequest(3, 25);

        request.Skip.Should().Be(50);
    }
}
