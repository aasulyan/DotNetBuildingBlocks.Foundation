namespace DotNetBuildingBlocks.Serialization;

/// <summary>
/// Represents the shared JSON serializer behavior used by DotNetBuildingBlocks.
/// </summary>
public static class JsonSerializerDefaults
{
    /// <summary>
    /// Gets the default property name case-insensitive behavior.
    /// </summary>
    public const bool PropertyNameCaseInsensitive = true;

    /// <summary>
    /// Gets the default indentation behavior.
    /// </summary>
    public const bool WriteIndented = false;
}
