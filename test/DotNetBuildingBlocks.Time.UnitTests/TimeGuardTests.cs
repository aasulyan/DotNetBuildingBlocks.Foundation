namespace DotNetBuildingBlocks.Time.UnitTests;

using FluentAssertions;
using Xunit;

public sealed class TimeGuardTests
{
    [Fact]
    public void AgainstNonUtc_ForDateTime_Should_Return_Value_When_Utc()
    {
        DateTime input = new(2026, 1, 1, 12, 30, 0, DateTimeKind.Utc);

        DateTime result = TimeGuard.AgainstNonUtc(input, nameof(input));

        result.Should().Be(input);
    }

    [Fact]
    public void AgainstNonUtc_ForDateTime_Should_Throw_When_Local()
    {
        DateTime input = new(2026, 1, 1, 12, 30, 0, DateTimeKind.Local);

        Action action = () => TimeGuard.AgainstNonUtc(input, nameof(input));

        action.Should()
            .Throw<ArgumentException>()
            .Where(ex => ex.ParamName == nameof(input) && ex.Message.Contains("DateTime value must use DateTimeKind.Utc."));
    }

    [Fact]
    public void AgainstNonUtc_ForDateTime_Should_Throw_When_Unspecified()
    {
        DateTime input = new(2026, 1, 1, 12, 30, 0, DateTimeKind.Unspecified);

        Action action = () => TimeGuard.AgainstNonUtc(input, nameof(input));

        action.Should()
            .Throw<ArgumentException>()
            .Where(ex => ex.ParamName == nameof(input) && ex.Message.Contains("DateTime value must use DateTimeKind.Utc."));
    }

    [Fact]
    public void AgainstNonUtc_ForDateTimeOffset_Should_Return_Value_When_Utc()
    {
        DateTimeOffset input = new(2026, 1, 1, 12, 30, 0, TimeSpan.Zero);

        DateTimeOffset result = TimeGuard.AgainstNonUtc(input, nameof(input));

        result.Should().Be(input);
    }

    [Fact]
    public void AgainstNonUtc_ForDateTimeOffset_Should_Throw_When_Offset_Is_Not_Utc()
    {
        DateTimeOffset input = new(2026, 1, 1, 12, 30, 0, TimeSpan.FromHours(4));

        Action action = () => TimeGuard.AgainstNonUtc(input, nameof(input));

        action.Should()
            .Throw<ArgumentException>()
            .Where(ex => ex.ParamName == nameof(input) && ex.Message.Contains("DateTimeOffset value must use the UTC offset."));
    }
}
