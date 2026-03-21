namespace DotNetBuildingBlocks.Time;

/// <summary>
/// Provides access to the system clock using UTC-based values.
/// </summary>
public sealed class SystemClock : IClock
{
    /// <summary>
    /// Gets the current UTC date and time.
    /// </summary>
    public DateTime UtcNow => DateTime.UtcNow;

    /// <summary>
    /// Gets the current UTC date and time with offset information.
    /// </summary>
    public DateTimeOffset UtcNowOffset => DateTimeOffset.UtcNow;

    /// <summary>
    /// Gets the current UTC calendar date.
    /// </summary>
    public DateOnly TodayUtc => DateOnly.FromDateTime(DateTime.UtcNow);
}
