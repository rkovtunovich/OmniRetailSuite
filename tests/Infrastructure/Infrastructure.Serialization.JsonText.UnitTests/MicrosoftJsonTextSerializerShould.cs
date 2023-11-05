using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace Infrastructure.Serialization.JsonText.UnitTests;

public class MicrosoftJsonTextSerializerShould
{
    [Fact]
    public void Serialize_ObjectWithJsonStringField_SerializedObject()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        var serializer = new MicrosoftJsonTextSerializer(Options.Create(options));

        var obj = new TestObject
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Status = TestStatus.Active,
            JsonString = JsonSerializer.Serialize(new { Id = Guid.NewGuid(), Name = "Test" })
        };

        // Act
        var serializedObj = serializer.Serialize(obj);

        // Assert
        serializedObj.Should().NotBeNullOrEmpty();
        serializedObj.Should().Contain(obj.Id.ToString());
        serializedObj.Should().Contain(obj.Name);
        serializedObj.Should().Contain(obj.Status.ToString());
    }
}

public class TestObject
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public TestStatus Status { get; set; }

    public string? JsonString { get; set; }
}

public enum TestStatus
{
    Active,
    Inactive
}
