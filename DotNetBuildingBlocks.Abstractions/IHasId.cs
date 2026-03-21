namespace DotNetBuildingBlocks.Abstractions;

/// <summary>
/// Represents an object that exposes a strongly typed identifier.
/// </summary>
/// <typeparam name="TId">The identifier type.</typeparam>
public interface IHasId<out TId>
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    TId Id { get; }
}
