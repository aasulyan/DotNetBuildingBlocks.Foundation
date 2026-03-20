namespace DotNetBuildingBlocks.Abstractions.Abstractions;

/// <summary>
/// Represents a minimal paging query contract.
/// </summary>
public interface IPagedQuery
{
    /// <summary>
    /// Gets the 1-based page number.
    /// </summary>
    int PageNumber { get; }

    /// <summary>
    /// Gets the requested page size.
    /// </summary>
    int PageSize { get; }
}
