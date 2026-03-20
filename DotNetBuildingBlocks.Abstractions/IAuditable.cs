namespace DotNetBuildingBlocks.Abstractions.Abstractions;

/// <summary>
/// Represents neutral audit information for create and update operations.
/// </summary>
public interface IAuditable
{
    /// <summary>
    /// Gets or sets the UTC timestamp when the object was created.
    /// </summary>
    DateTimeOffset CreatedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the actor who created the object.
    /// </summary>
    string? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the UTC timestamp of the last modification.
    /// </summary>
    DateTimeOffset? LastModifiedAtUtc { get; set; }

    /// <summary>
    /// Gets or sets the actor who last modified the object.
    /// </summary>
    string? LastModifiedBy { get; set; }
}
