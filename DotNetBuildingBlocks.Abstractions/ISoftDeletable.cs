namespace DotNetBuildingBlocks.Abstractions.Abstractions;

/// <summary>
/// Represents a neutral soft-delete contract.
/// </summary>
public interface ISoftDeletable
{
    /// <summary>
    /// Gets or sets a value indicating whether the object is logically deleted.
    /// </summary>
    bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp when the object was marked as deleted.
    /// </summary>
    DateTimeOffset? DeletedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the actor who marked the object as deleted.
    /// </summary>
    string? DeletedBy { get; set; }
}
