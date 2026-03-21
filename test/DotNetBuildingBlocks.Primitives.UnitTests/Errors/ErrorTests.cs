using DotNetBuildingBlocks.Primitives.Errors;
using FluentAssertions;
using Xunit;

namespace DotNetBuildingBlocks.Primitives.UnitTests.Errors;

public sealed class ErrorTests
{
    [Fact]
    public void Create_Should_Return_Error_When_Values_Are_Valid()
    {
        var error = Error.Create("sample.code", "Sample message");

        error.Code.Should().Be("sample.code");
        error.Message.Should().Be("Sample message");
    }

    [Fact]
    public void None_Should_Be_Empty_Error()
    {
        Error.None.Code.Should().BeEmpty();
        Error.None.Message.Should().BeEmpty();
    }
}
