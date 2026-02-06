using System.Text.Json;
using Microsoft.Extensions.Logging;
using Viamus.Sonarqube.Mcp.Server.Models;
using Viamus.Sonarqube.Mcp.Server.Services;
using Viamus.Sonarqube.Mcp.Server.Tools;

namespace Viamus.Sonarqube.Mcp.Server.Tests.Tools;

public class HotspotToolsTests
{
    private readonly ISonarQubeClient _client = Substitute.For<ISonarQubeClient>();
    private readonly HotspotTools _tools;

    public HotspotToolsTests()
    {
        _tools = new HotspotTools(_client, Substitute.For<ILogger<HotspotTools>>());
    }

    [Fact]
    public async Task SearchHotspots_ShouldReturnSerializedResponse()
    {
        var response = new HotspotSearchResponse(
            new Paging(1, 100, 1),
            [new Hotspot("hotspot-1", "my-project:File.cs", "my-project", "sql-injection", "HIGH",
                "TO_REVIEW", 10, "Review this", null, null, null)],
            null);

        _client.SearchHotspotsAsync("my-project", Arg.Any<string?>(), Arg.Any<int?>(), Arg.Any<int?>(), Arg.Any<CancellationToken>())
            .Returns(response);

        var result = await _tools.search_hotspots("my-project");

        var deserialized = JsonSerializer.Deserialize<HotspotSearchResponse>(result);
        deserialized.Should().NotBeNull();
        deserialized!.Hotspots.Should().HaveCount(1);
        deserialized.Hotspots[0].Key.Should().Be("hotspot-1");
    }

    [Fact]
    public async Task SearchHotspots_WhenClientThrows_ShouldPropagateException()
    {
        _client.SearchHotspotsAsync(Arg.Any<string>(), Arg.Any<string?>(), Arg.Any<int?>(), Arg.Any<int?>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Connection failed"));

        var act = () => _tools.search_hotspots("my-project");

        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Fact]
    public async Task GetHotspot_ShouldReturnSerializedResponse()
    {
        var response = new HotspotDetailResponse(
            "hotspot-1",
            new HotspotDetailComponent("my-project:File.cs", "FIL", "File.cs", "src/File.cs"),
            new HotspotDetailProject("my-project", "My Project"),
            new HotspotDetailRule("cs:S1234", "Rule Name", "sql-injection", "HIGH"),
            "TO_REVIEW", "Review this", 10, "sql-injection", "HIGH", null, null, null, null);

        _client.GetHotspotAsync("hotspot-1", Arg.Any<CancellationToken>())
            .Returns(response);

        var result = await _tools.get_hotspot("hotspot-1");

        var deserialized = JsonSerializer.Deserialize<HotspotDetailResponse>(result);
        deserialized.Should().NotBeNull();
        deserialized!.Key.Should().Be("hotspot-1");
        deserialized.Rule.Key.Should().Be("cs:S1234");
    }

    [Fact]
    public async Task GetHotspot_WhenClientThrows_ShouldPropagateException()
    {
        _client.GetHotspotAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Connection failed"));

        var act = () => _tools.get_hotspot("hotspot-1");

        await act.Should().ThrowAsync<HttpRequestException>();
    }
}
