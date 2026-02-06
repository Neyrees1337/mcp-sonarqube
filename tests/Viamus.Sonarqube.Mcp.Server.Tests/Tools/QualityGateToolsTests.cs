using System.Text.Json;
using Microsoft.Extensions.Logging;
using Viamus.Sonarqube.Mcp.Server.Models;
using Viamus.Sonarqube.Mcp.Server.Services;
using Viamus.Sonarqube.Mcp.Server.Tools;

namespace Viamus.Sonarqube.Mcp.Server.Tests.Tools;

public class QualityGateToolsTests
{
    private readonly ISonarQubeClient _client = Substitute.For<ISonarQubeClient>();
    private readonly QualityGateTools _tools;

    public QualityGateToolsTests()
    {
        _tools = new QualityGateTools(_client, Substitute.For<ILogger<QualityGateTools>>());
    }

    [Fact]
    public async Task ListQualityGates_ShouldReturnSerializedResponse()
    {
        var response = new QualityGateListResponse(
            [new QualityGate(1, "Sonar way", true, true, null)],
            1);

        _client.ListQualityGatesAsync(Arg.Any<CancellationToken>())
            .Returns(response);

        var result = await _tools.list_quality_gates();

        var deserialized = JsonSerializer.Deserialize<QualityGateListResponse>(result);
        deserialized.Should().NotBeNull();
        deserialized!.QualityGates.Should().HaveCount(1);
        deserialized.QualityGates[0].Name.Should().Be("Sonar way");
    }

    [Fact]
    public async Task ListQualityGates_WhenClientThrows_ShouldPropagateException()
    {
        _client.ListQualityGatesAsync(Arg.Any<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Connection failed"));

        var act = () => _tools.list_quality_gates();

        await act.Should().ThrowAsync<HttpRequestException>();
    }
}
