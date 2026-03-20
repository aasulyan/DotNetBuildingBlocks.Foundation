namespace DotNetBuildingBlocks.Abstractions.Abstractions;

/// <summary>
/// Represents a minimal persistence-neutral entity contract.
/// </summary>
/// <typeparam name="TId">The identifier type.</typeparam>
public interface IEntity<out TId> : IHasId<TId>
{
}
