using System.Text.Json;
using Microsoft.Extensions.Logging;
using Viamus.Sonarqube.Mcp.Server.Models;
using Viamus.Sonarqube.Mcp.Server.Services;
using Viamus.Sonarqube.Mcp.Server.Tools;

namespace Viamus.Sonarqube.Mcp.Server.Tests.Tools;

public class MeasureToolsTests
{
    private readonly ISonarQubeClient _client = Substitute.For<ISonarQubeClient>();
    private readonly MeasureTools _tools;

    public MeasureToolsTests()
    {
        _tools = new MeasureTools(_client, Substitute.For<ILogger<MeasureTools>>());
    }

    [Fact]
    public async Task GetMeasures_ShouldReturnSerializedResponse()
    {
        var response = new MeasureComponentResponse(
            new MeasureComponent("my-project", "My Project", "TRK",
                [new Measure("coverage", "85.5", null), new Measure("bugs", "3", null)]));

        _client.GetMeasuresAsync("my-project", "coverage,bugs", Arg.Any<CancellationToken>())
            .Returns(response);

        var result = await _tools.get_measures("my-project", "coverage,bugs");

        var deserialized = JsonSerializer.Deserialize<MeasureComponentResponse>(result);
        deserialized.Should().NotBeNull();
        deserialized!.Component.Measures.Should().HaveCount(2);
        deserialized.Component.Measures![0].Metric.Should().Be("coverage");
    }

    [Fact]
    public async Task GetMeasures_WhenClientThrows_ShouldPropagateException()
    {
        _client.GetMeasuresAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Connection failed"));

        var act = () => _tools.get_measures("my-project", "coverage");

        await act.Should().ThrowAsync<HttpRequestException>();
    }
}
