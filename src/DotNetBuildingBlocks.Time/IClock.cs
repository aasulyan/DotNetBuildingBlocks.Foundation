namespace DotNetBuildingBlocks.Time;

/// <summary>
/// Represents an abstraction for accessing the current UTC time in a test-friendly way.
/// </summary>
public interface IClock
{
    /// <summary>
    /// Gets the current UTC date and time.
    /// </summary>
    DateTime UtcNow { get; }

    /// <summary>
    /// Gets the current UTC date and time with offset information.
    /// </summary>
    DateTimeOffset UtcNowOffset { get; }

    /// <summary>
    /// Gets the current UTC calendar date.
    /// </summary>
    DateOnly TodayUtc { get; }
}
