namespace DotNetBuildingBlocks.Serialization.UnitTests;

using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Xunit;

public sealed class JsonSerializerConfigurationExtensionsTests
{
    [Fact]
    public void ApplyDotNetBuildingBlocksDefaults_Should_Configure_Options()
    {
        JsonSerializerOptions options = new();

        JsonSerializerOptions result = options.ApplyDotNetBuildingBlocksDefaults();

        result.Should().BeSameAs(options);
        options.PropertyNamingPolicy.Should().BeSameAs(JsonNamingPolicy.CamelCase);
        options.DictionaryKeyPolicy.Should().BeSameAs(JsonNamingPolicy.CamelCase);
        options.Converters.OfType<JsonStringEnumConverter>().Should().ContainSingle();
    }

    [Fact]
    public void CreateDotNetBuildingBlocksDefaults_Should_Return_New_Configured_Options()
    {
        JsonSerializerOptions options = JsonSerializerConfigurationExtensions.CreateDotNetBuildingBlocksDefaults();

        options.PropertyNamingPolicy.Should().BeSameAs(JsonNamingPolicy.CamelCase);
        options.DictionaryKeyPolicy.Should().BeSameAs(JsonNamingPolicy.CamelCase);
        options.PropertyNameCaseInsensitive.Should().BeTrue();
        options.DefaultIgnoreCondition.Should().Be(JsonIgnoreCondition.WhenWritingNull);
    }
}
