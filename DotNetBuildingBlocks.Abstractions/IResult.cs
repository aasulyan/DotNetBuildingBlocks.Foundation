namespace DotNetBuildingBlocks.Abstractions.Abstractions;

/// <summary>
/// Represents the minimal shared contract for a result-like type.
/// </summary>
public interface IResult
{
    /// <summary>
    /// Gets a value indicating whether the operation succeeded.
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the operation failed.
    /// </summary>
    bool IsFailure => !IsSuccess;
}
