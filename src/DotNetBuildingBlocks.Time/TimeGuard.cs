namespace DotNetBuildingBlocks.Time;

/// <summary>
/// Provides lightweight helpers for validating time-related invariants.
/// </summary>
public static class TimeGuard
{
    /// <summary>
    /// Ensures that the specified <see cref="DateTime" /> uses the UTC kind.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">The parameter name.</param>
    /// <returns>The validated UTC value.</returns>
    /// <exception cref="ArgumentException">Thrown when the value is not UTC.</exception>
    public static DateTime AgainstNonUtc(DateTime value, string? paramName = null)
    {
        if (value.Kind != DateTimeKind.Utc)
        {
            throw new ArgumentException("DateTime value must use DateTimeKind.Utc.", paramName);
        }

        return value;
    }

    /// <summary>
    /// Ensures that the specified <see cref="DateTimeOffset" /> uses the UTC offset.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">The parameter name.</param>
    /// <returns>The validated UTC value.</returns>
    /// <exception cref="ArgumentException">Thrown when the value does not use the UTC offset.</exception>
    public static DateTimeOffset AgainstNonUtc(DateTimeOffset value, string? paramName = null)
    {
        if (value.Offset != TimeSpan.Zero)
        {
            throw new ArgumentException("DateTimeOffset value must use the UTC offset.", paramName);
        }

        return value;
    }
}
