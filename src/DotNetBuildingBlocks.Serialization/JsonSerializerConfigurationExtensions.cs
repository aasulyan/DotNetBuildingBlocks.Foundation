namespace DotNetBuildingBlocks.Serialization;

using System.Text.Json;

/// <summary>
/// Provides extension methods for working with shared JSON serializer configuration.
/// </summary>
public static class JsonSerializerConfigurationExtensions
{
    /// <summary>
    /// Applies DotNetBuildingBlocks defaults to the specified <see cref="JsonSerializerOptions" />.
    /// </summary>
    /// <param name="options">The options instance to configure.</param>
    /// <returns>The configured options instance.</returns>
    public static JsonSerializerOptions ApplyDotNetBuildingBlocksDefaults(this JsonSerializerOptions options)
    {
        return JsonSerializerConfiguration.ApplyDefaults(options);
    }

    /// <summary>
    /// Creates a copy of the shared DotNetBuildingBlocks default <see cref="JsonSerializerOptions" />.
    /// </summary>
    /// <returns>A new configured <see cref="JsonSerializerOptions" /> instance.</returns>
    public static JsonSerializerOptions CreateDotNetBuildingBlocksDefaults()
    {
        return JsonSerializerConfiguration.CreateDefault();
    }
}
