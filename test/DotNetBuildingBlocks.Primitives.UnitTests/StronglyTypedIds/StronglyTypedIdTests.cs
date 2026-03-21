using DotNetBuildingBlocks.Primitives.StronglyTypedIds;
using FluentAssertions;
using Xunit;

namespace DotNetBuildingBlocks.Primitives.UnitTests.StronglyTypedIds;

public sealed class StronglyTypedIdTests
{
    [Fact]
    public void ToString_Should_Return_Underlying_Value_Text()
    {
        var id = new CustomerId(Guid.Parse("11111111-1111-1111-1111-111111111111"));

        id.ToString().Should().Be("11111111-1111-1111-1111-111111111111");
    }

    private sealed record CustomerId(Guid Value) : StronglyTypedId<Guid>(Value);
}
