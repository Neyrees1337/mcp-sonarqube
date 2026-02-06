using System.Text.Json;
using Microsoft.Extensions.Logging;
using Viamus.Sonarqube.Mcp.Server.Models;
using Viamus.Sonarqube.Mcp.Server.Services;
using Viamus.Sonarqube.Mcp.Server.Tools;

namespace Viamus.Sonarqube.Mcp.Server.Tests.Tools;

public class ProjectToolsTests
{
    private readonly ISonarQubeClient _client = Substitute.For<ISonarQubeClient>();
    private readonly ProjectTools _tools;

    public ProjectToolsTests()
    {
        _tools = new ProjectTools(_client, Substitute.For<ILogger<ProjectTools>>());
    }

    [Fact]
    public async Task SearchProjects_ShouldReturnSerializedResponse()
    {
        var response = new ProjectSearchResponse(
            new Paging(1, 100, 1),
            [new Project("my-project", "My Project", "TRK", "public", "2024-01-01", null)]);

        _client.SearchProjectsAsync(Arg.Any<string?>(), Arg.Any<int?>(), Arg.Any<int?>(), Arg.Any<CancellationToken>())
            .Returns(response);

        var result = await _tools.search_projects("my-project");

        var deserialized = JsonSerializer.Deserialize<ProjectSearchResponse>(result);
        deserialized.Should().NotBeNull();
        deserialized!.Components.Should().HaveCount(1);
        deserialized.Components[0].Key.Should().Be("my-project");
    }

    [Fact]
    public async Task SearchProjects_WhenClientThrows_ShouldPropagateException()
    {
        _client.SearchProjectsAsync(Arg.Any<string?>(), Arg.Any<int?>(), Arg.Any<int?>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Connection failed"));

        var act = () => _tools.search_projects();

        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Fact]
    public async Task GetProjectStatus_ShouldReturnCombinedResponse()
    {
        var qualityGateResponse = new QualityGateProjectStatusResponse(
            new ProjectQualityGateStatus("OK", [new QualityGateCondition("OK", "coverage", "LT", "80", "85")]));

        var measuresResponse = new MeasureComponentResponse(
            new MeasureComponent("my-project", "My Project", "TRK",
                [new Measure("coverage", "85", null)]));

        _client.GetQualityGateProjectStatusAsync("my-project", Arg.Any<CancellationToken>())
            .Returns(qualityGateResponse);
        _client.GetMeasuresAsync("my-project", Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(measuresResponse);

        var result = await _tools.get_project_status("my-project");

        var doc = JsonDocument.Parse(result);
        doc.RootElement.GetProperty("qualityGate").Should().NotBeNull();
        doc.RootElement.GetProperty("measures").Should().NotBeNull();
    }
}
