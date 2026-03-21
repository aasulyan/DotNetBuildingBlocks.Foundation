namespace DotNetBuildingBlocks.Guards;

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using DotNetBuildingBlocks.Guards.Internal;

/// <summary>
/// Provides lightweight guard clauses for validating arguments and protecting invariants.
/// </summary>
public static class Guard
{
    /// <summary>
    /// Ensures that a reference type argument is not null.
    /// </summary>
    /// <typeparam name="T">The argument type.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">The argument name.</param>
    /// <returns>The validated value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when value is null.</exception>
    [return: NotNull]
    public static T AgainstNull<T>(
        [NotNull] T? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : class
    {
        if (value is null)
        {
            ThrowHelper.ThrowArgumentNull(paramName);
        }

        return value;
    }

    /// <summary>
    /// Ensures that a nullable value type argument is not null.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">The argument name.</param>
    /// <returns>The validated non-null value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when value is null.</exception>
    public static T AgainstNull<T>(
        T? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct
    {
        if (!value.HasValue)
        {
            ThrowHelper.ThrowArgumentNull(paramName);
        }

        return value.Value;
    }

    /// <summary>
    /// Ensures that a string argument is not null or empty.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">The argument name.</param>
    /// <returns>The validated string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when value is null.</exception>
    /// <exception cref="ArgumentException">Thrown when value is empty.</exception>
    public static string AgainstNullOrEmpty(
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
        {
            ThrowHelper.ThrowArgumentNull(paramName);
        }

        if (value.Length == 0)
        {
            ThrowHelper.ThrowArgumentException("String cannot be empty.", paramName);
        }

        return value;
    }

    /// <summary>
    /// Ensures that a string argument is not null, empty, or whitespace.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">The argument name.</param>
    /// <returns>The validated string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when value is null.</exception>
    /// <exception cref="ArgumentException">Thrown when value is empty or whitespace.</exception>
    public static string AgainstNullOrWhiteSpace(
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
        {
            ThrowHelper.ThrowArgumentNull(paramName);
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            ThrowHelper.ThrowArgumentException("String cannot be empty or whitespace.", paramName);
        }

        return value;
    }

    /// <summary>
    /// Ensures that a value type argument is not its default value.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">The argument name.</param>
    /// <returns>The validated value.</returns>
    /// <exception cref="ArgumentException">Thrown when value equals its default value.</exception>
    public static T AgainstDefault<T>(
        T value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct
    {
        if (EqualityComparer<T>.Default.Equals(value, default))
        {
            ThrowHelper.ThrowArgumentException("Value cannot be the default value.", paramName);
        }

        return value;
    }

    /// <summary>
    /// Ensures that a Guid argument is not empty.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">The argument name.</param>
    /// <returns>The validated Guid.</returns>
    /// <exception cref="ArgumentException">Thrown when value is Guid.Empty.</exception>
    public static Guid AgainstEmpty(
        Guid value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value == Guid.Empty)
        {
            ThrowHelper.ThrowArgumentException("Guid cannot be empty.", paramName);
        }

        return value;
    }

    /// <summary>
    /// Ensures that a collection argument is not null or empty.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">The argument name.</param>
    /// <returns>The validated collection.</returns>
    /// <exception cref="ArgumentNullException">Thrown when value is null.</exception>
    /// <exception cref="ArgumentException">Thrown when value is empty.</exception>
    public static IReadOnlyCollection<T> AgainstNullOrEmpty<T>(
        IReadOnlyCollection<T>? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
        {
            ThrowHelper.ThrowArgumentNull(paramName);
        }

        if (value.Count == 0)
        {
            ThrowHelper.ThrowArgumentException("Collection cannot be empty.", paramName);
        }

        return value;
    }

    /// <summary>
    /// Ensures that a collection argument is not null or empty.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">The argument name.</param>
    /// <returns>The validated enumerable.</returns>
    /// <exception cref="ArgumentNullException">Thrown when value is null.</exception>
    /// <exception cref="ArgumentException">Thrown when value is empty.</exception>
    public static IEnumerable<T> AgainstNullOrEmpty<T>(
        IEnumerable<T>? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
        {
            ThrowHelper.ThrowArgumentNull(paramName);
        }

        if (value is ICollection collection && collection.Count == 0)
        {
            ThrowHelper.ThrowArgumentException("Collection cannot be empty.", paramName);
        }

        using IEnumerator<T> enumerator = value.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            ThrowHelper.ThrowArgumentException("Collection cannot be empty.", paramName);
        }

        return value;
    }

    /// <summary>
    /// Ensures that an integer value is within the specified inclusive range.
    /// </summary>
    public static int AgainstOutOfRange(
        int value,
        int minimum,
        int maximum,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (minimum > maximum)
        {
            ThrowHelper.ThrowArgumentException("Minimum cannot be greater than maximum.", nameof(minimum));
        }

        if (value < minimum || value > maximum)
        {
            ThrowHelper.ThrowArgumentOutOfRange(paramName, value, minimum, maximum);
        }

        return value;
    }

    /// <summary>
    /// Ensures that a long value is within the specified inclusive range.
    /// </summary>
    public static long AgainstOutOfRange(
        long value,
        long minimum,
        long maximum,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (minimum > maximum)
        {
            ThrowHelper.ThrowArgumentException("Minimum cannot be greater than maximum.", nameof(minimum));
        }

        if (value < minimum || value > maximum)
        {
            ThrowHelper.ThrowArgumentOutOfRange(paramName, value, minimum, maximum);
        }

        return value;
    }

    /// <summary>
    /// Ensures that a decimal value is within the specified inclusive range.
    /// </summary>
    public static decimal AgainstOutOfRange(
        decimal value,
        decimal minimum,
        decimal maximum,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (minimum > maximum)
        {
            ThrowHelper.ThrowArgumentException("Minimum cannot be greater than maximum.", nameof(minimum));
        }

        if (value < minimum || value > maximum)
        {
            ThrowHelper.ThrowArgumentOutOfRange(paramName, value, minimum, maximum);
        }

        return value;
    }

    /// <summary>
    /// Ensures that an integer value is not negative.
    /// </summary>
    public static int AgainstNegative(
        int value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value < 0)
        {
            ThrowHelper.ThrowArgumentOutOfRange(paramName, value, "Value cannot be negative.");
        }

        return value;
    }

    /// <summary>
    /// Ensures that a long value is not negative.
    /// </summary>
    public static long AgainstNegative(
        long value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value < 0)
        {
            ThrowHelper.ThrowArgumentOutOfRange(paramName, value, "Value cannot be negative.");
        }

        return value;
    }

    /// <summary>
    /// Ensures that a decimal value is not negative.
    /// </summary>
    public static decimal AgainstNegative(
        decimal value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value < 0)
        {
            ThrowHelper.ThrowArgumentOutOfRange(paramName, value, "Value cannot be negative.");
        }

        return value;
    }

    /// <summary>
    /// Ensures that an integer value is greater than zero.
    /// </summary>
    public static int AgainstZeroOrNegative(
        int value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value <= 0)
        {
            ThrowHelper.ThrowArgumentOutOfRange(paramName, value, "Value must be greater than zero.");
        }

        return value;
    }

    /// <summary>
    /// Ensures that a long value is greater than zero.
    /// </summary>
    public static long AgainstZeroOrNegative(
        long value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value <= 0)
        {
            ThrowHelper.ThrowArgumentOutOfRange(paramName, value, "Value must be greater than zero.");
        }

        return value;
    }

    /// <summary>
    /// Ensures that a decimal value is greater than zero.
    /// </summary>
    public static decimal AgainstZeroOrNegative(
        decimal value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value <= 0)
        {
            ThrowHelper.ThrowArgumentOutOfRange(paramName, value, "Value must be greater than zero.");
        }

        return value;
    }
}
