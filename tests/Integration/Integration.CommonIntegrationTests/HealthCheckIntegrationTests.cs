using HealthChecks.UI.Core;
using Infrastructure.Serialization.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.CommonIntegrationTests;

public class HealthCheckIntegrationTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task Client_Should_Get_And_Deserialize_HealthCheckResponse()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/_health");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        responseString.Should().NotBeNullOrEmpty();

        var serializer = factory.Services.GetRequiredService<IDataSerializer>();
        serializer.Should().NotBeNull();

        var healthCheck = serializer.Deserialize<UIHealthReport>(responseString);
        healthCheck.Should().NotBeNull();
        healthCheck?.Status.Should().Be(UIHealthStatus.Healthy);
    }
}
