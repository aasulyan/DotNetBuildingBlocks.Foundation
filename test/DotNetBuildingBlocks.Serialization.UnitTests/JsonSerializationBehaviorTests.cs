namespace DotNetBuildingBlocks.Serialization.UnitTests;

using System.Text.Json;
using FluentAssertions;
using Xunit;

public sealed class JsonSerializationBehaviorTests
{
    [Fact]
    public void Serialize_Should_Use_CamelCase_Property_Names()
    {
        SampleRequest request = new("Arsen", UserRole.Admin, null);

        string json = JsonSerializer.Serialize(request, JsonSerializerConfiguration.Default);

        json.Should().Contain("\"firstName\":\"Arsen\"");
        json.Should().Contain("\"role\":\"admin\"");
    }

    [Fact]
    public void Serialize_Should_Ignore_Null_Values()
    {
        SampleRequest request = new("Arsen", UserRole.Admin, null);

        string json = JsonSerializer.Serialize(request, JsonSerializerConfiguration.Default);

        json.Should().NotContain("optionalNote");
    }

    [Fact]
    public void Deserialize_Should_Be_Case_Insensitive()
    {
        const string json = """
                            {
                              "FIRSTNAME": "Arsen",
                              "ROLE": "admin"
                            }
                            """;

        SampleRequest? result = JsonSerializer.Deserialize<SampleRequest>(json, JsonSerializerConfiguration.Default);

        result.Should().NotBeNull();
        result!.FirstName.Should().Be("Arsen");
        result.Role.Should().Be(UserRole.Admin);
    }

    private sealed record SampleRequest(string FirstName, UserRole Role, string? OptionalNote);

    private enum UserRole
    {
        Unknown = 0,
        Admin = 1,
    }
}
