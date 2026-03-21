namespace DotNetBuildingBlocks.Time.UnitTests;

using FluentAssertions;
using Xunit;

public sealed class SystemClockTests
{
    [Fact]
    public void UtcNow_Should_Return_Utc_DateTime()
    {
        SystemClock clock = new();

        DateTime value = clock.UtcNow;

        value.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Fact]
    public void UtcNowOffset_Should_Return_Zero_Offset()
    {
        SystemClock clock = new();

        DateTimeOffset value = clock.UtcNowOffset;

        value.Offset.Should().Be(TimeSpan.Zero);
    }

    [Fact]
    public void TodayUtc_Should_Match_Current_Utc_Date()
    {
        SystemClock clock = new();

        DateOnly today = clock.TodayUtc;
        DateOnly expected = DateOnly.FromDateTime(DateTime.UtcNow);

        today.Should().Be(expected);
    }
}
