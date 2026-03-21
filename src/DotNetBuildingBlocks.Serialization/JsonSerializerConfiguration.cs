namespace DotNetBuildingBlocks.Serialization;

using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Provides shared <see cref="JsonSerializerOptions" /> configuration for DotNetBuildingBlocks.
/// </summary>
public static class JsonSerializerConfiguration
{
    private static readonly JsonSerializerOptions SharedDefault = CreateCore();

    /// <summary>
    /// Gets a shared read-only-like default options instance.
    /// </summary>
    /// <remarks>
    /// Consumers should not mutate this instance. Use <see cref="CreateDefault" /> to get a copy.
    /// </remarks>
    public static JsonSerializerOptions Default => SharedDefault;

    /// <summary>
    /// Creates a new <see cref="JsonSerializerOptions" /> instance with DotNetBuildingBlocks defaults.
    /// </summary>
    /// <returns>A new configured <see cref="JsonSerializerOptions" /> instance.</returns>
    public static JsonSerializerOptions CreateDefault()
    {
        return new JsonSerializerOptions(SharedDefault);
    }

    /// <summary>
    /// Applies DotNetBuildingBlocks defaults to the specified <see cref="JsonSerializerOptions" /> instance.
    /// </summary>
    /// <param name="options">The options instance to configure.</param>
    /// <returns>The configured options instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="options" /> is null.</exception>
    public static JsonSerializerOptions ApplyDefaults(JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        options.PropertyNameCaseInsensitive = JsonSerializerDefaults.PropertyNameCaseInsensitive;
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.WriteIndented = JsonSerializerDefaults.WriteIndented;

        EnsureStringEnumConverter(options);

        return options;
    }

    private static JsonSerializerOptions CreateCore()
    {
        JsonSerializerOptions options = new();
        ApplyDefaults(options);
        return options;
    }

    private static void EnsureStringEnumConverter(JsonSerializerOptions options)
    {
        bool hasConverter = options.Converters.OfType<JsonStringEnumConverter>().Any();
        if (!hasConverter)
        {
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        }
    }
}
