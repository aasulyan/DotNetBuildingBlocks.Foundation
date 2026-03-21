namespace DotNetBuildingBlocks.Time.UnitTests;

using FluentAssertions;
using Xunit;

public sealed class FakeClockTests
{
    [Fact]
    public void FakeClock_Should_Return_Configured_Values()
    {
        DateTime utcNow = new(2026, 2, 15, 8, 45, 0, DateTimeKind.Utc);
        DateTimeOffset utcNowOffset = new(2026, 2, 15, 8, 45, 0, TimeSpan.Zero);

        IClock clock = new FakeClock(utcNow, utcNowOffset);

        clock.UtcNow.Should().Be(utcNow);
        clock.UtcNowOffset.Should().Be(utcNowOffset);
        clock.TodayUtc.Should().Be(DateOnly.FromDateTime(utcNow));
    }

    private sealed class FakeClock : IClock
    {
        public FakeClock(DateTime utcNow, DateTimeOffset utcNowOffset)
        {
            UtcNow = utcNow;
            UtcNowOffset = utcNowOffset;
        }

        public DateTime UtcNow { get; }

        public DateTimeOffset UtcNowOffset { get; }

        public DateOnly TodayUtc => DateOnly.FromDateTime(UtcNow);
    }
}
