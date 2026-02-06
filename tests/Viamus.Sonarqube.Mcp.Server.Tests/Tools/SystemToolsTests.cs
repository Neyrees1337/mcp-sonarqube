using System.Text.Json;
using Microsoft.Extensions.Logging;
using Viamus.Sonarqube.Mcp.Server.Models;
using Viamus.Sonarqube.Mcp.Server.Services;
using Viamus.Sonarqube.Mcp.Server.Tools;

namespace Viamus.Sonarqube.Mcp.Server.Tests.Tools;

public class SystemToolsTests
{
    private readonly ISonarQubeClient _client = Substitute.For<ISonarQubeClient>();
    private readonly SystemTools _tools;

    public SystemToolsTests()
    {
        _tools = new SystemTools(_client, Substitute.For<ILogger<SystemTools>>());
    }

    [Fact]
    public async Task GetHealth_ShouldReturnSerializedResponse()
    {
        var response = new SystemHealthResponse("GREEN", []);

        _client.GetSystemHealthAsync(Arg.Any<CancellationToken>())
            .Returns(response);

        var result = await _tools.get_health();

        var deserialized = JsonSerializer.Deserialize<SystemHealthResponse>(result);
        deserialized.Should().NotBeNull();
        deserialized!.Health.Should().Be("GREEN");
    }

    [Fact]
    public async Task GetHealth_WhenClientThrows_ShouldPropagateException()
    {
        _client.GetSystemHealthAsync(Arg.Any<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Connection failed"));

        var act = () => _tools.get_health();

        await act.Should().ThrowAsync<HttpRequestException>();
    }
}
