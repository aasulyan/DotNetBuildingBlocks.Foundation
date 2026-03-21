namespace DotNetBuildingBlocks.Guards.Internal;

using System.Diagnostics.CodeAnalysis;

internal static class ThrowHelper
{
    [DoesNotReturn]
    public static void ThrowArgumentNull(string? paramName)
    {
        throw new ArgumentNullException(paramName);
    }

    [DoesNotReturn]
    public static void ThrowArgumentException(string message, string? paramName)
    {
        throw new ArgumentException(message, paramName);
    }

    [DoesNotReturn]
    public static void ThrowArgumentOutOfRange(string? paramName, int actualValue, int minimum, int maximum)
    {
        throw new ArgumentOutOfRangeException(
            paramName,
            actualValue,
            $"Value must be between {minimum} and {maximum}.");
    }

    [DoesNotReturn]
    public static void ThrowArgumentOutOfRange(string? paramName, long actualValue, long minimum, long maximum)
    {
        throw new ArgumentOutOfRangeException(
            paramName,
            actualValue,
            $"Value must be between {minimum} and {maximum}.");
    }

    [DoesNotReturn]
    public static void ThrowArgumentOutOfRange(string? paramName, decimal actualValue, decimal minimum, decimal maximum)
    {
        throw new ArgumentOutOfRangeException(
            paramName,
            actualValue,
            $"Value must be between {minimum} and {maximum}.");
    }

    [DoesNotReturn]
    public static void ThrowArgumentOutOfRange(string? paramName, int actualValue, string message)
    {
        throw new ArgumentOutOfRangeException(paramName, actualValue, message);
    }

    [DoesNotReturn]
    public static void ThrowArgumentOutOfRange(string? paramName, long actualValue, string message)
    {
        throw new ArgumentOutOfRangeException(paramName, actualValue, message);
    }

    [DoesNotReturn]
    public static void ThrowArgumentOutOfRange(string? paramName, decimal actualValue, string message)
    {
        throw new ArgumentOutOfRangeException(paramName, actualValue, message);
    }
}
