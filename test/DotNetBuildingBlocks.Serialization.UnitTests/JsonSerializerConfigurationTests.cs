namespace DotNetBuildingBlocks.Serialization.UnitTests;

using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Xunit;

public sealed class JsonSerializerConfigurationTests
{
    [Fact]
    public void CreateDefault_Should_Return_Configured_Options()
    {
        JsonSerializerOptions options = JsonSerializerConfiguration.CreateDefault();

        options.PropertyNamingPolicy.Should().BeSameAs(JsonNamingPolicy.CamelCase);
        options.DictionaryKeyPolicy.Should().BeSameAs(JsonNamingPolicy.CamelCase);
        options.PropertyNameCaseInsensitive.Should().BeTrue();
        options.DefaultIgnoreCondition.Should().Be(JsonIgnoreCondition.WhenWritingNull);
        options.WriteIndented.Should().BeFalse();
        options.Converters.OfType<JsonStringEnumConverter>().Should().ContainSingle();
    }

    [Fact]
    public void Default_Should_Return_Same_Shared_Instance()
    {
        JsonSerializerOptions first = JsonSerializerConfiguration.Default;
        JsonSerializerOptions second = JsonSerializerConfiguration.Default;

        ReferenceEquals(first, second).Should().BeTrue();
    }

    [Fact]
    public void CreateDefault_Should_Return_New_Copy()
    {
        JsonSerializerOptions first = JsonSerializerConfiguration.CreateDefault();
        JsonSerializerOptions second = JsonSerializerConfiguration.CreateDefault();

        ReferenceEquals(first, second).Should().BeFalse();
        first.PropertyNameCaseInsensitive.Should().Be(second.PropertyNameCaseInsensitive);
    }

    [Fact]
    public void ApplyDefaults_Should_Throw_When_Options_Is_Null()
    {
        Action action = () => JsonSerializerConfiguration.ApplyDefaults(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ApplyDefaults_Should_Not_Add_Duplicate_String_Enum_Converter()
    {
        JsonSerializerOptions options = new();
        options.Converters.Add(new JsonStringEnumConverter());

        JsonSerializerConfiguration.ApplyDefaults(options);

        options.Converters.OfType<JsonStringEnumConverter>().Should().ContainSingle();
    }

    [Fact]
    public void ApplyDefaults_Should_Configure_Provided_Options_Instance()
    {
        JsonSerializerOptions options = new();

        JsonSerializerOptions result = JsonSerializerConfiguration.ApplyDefaults(options);

        result.Should().BeSameAs(options);
        options.PropertyNamingPolicy.Should().BeSameAs(JsonNamingPolicy.CamelCase);
        options.DictionaryKeyPolicy.Should().BeSameAs(JsonNamingPolicy.CamelCase);
        options.PropertyNameCaseInsensitive.Should().BeTrue();
        options.DefaultIgnoreCondition.Should().Be(JsonIgnoreCondition.WhenWritingNull);
        options.WriteIndented.Should().BeFalse();
    }
}
