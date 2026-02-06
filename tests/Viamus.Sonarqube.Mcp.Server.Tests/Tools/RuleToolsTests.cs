using System.Text.Json;
using Microsoft.Extensions.Logging;
using Viamus.Sonarqube.Mcp.Server.Models;
using Viamus.Sonarqube.Mcp.Server.Services;
using Viamus.Sonarqube.Mcp.Server.Tools;

namespace Viamus.Sonarqube.Mcp.Server.Tests.Tools;

public class RuleToolsTests
{
    private readonly ISonarQubeClient _client = Substitute.For<ISonarQubeClient>();
    private readonly RuleTools _tools;

    public RuleToolsTests()
    {
        _tools = new RuleTools(_client, Substitute.For<ILogger<RuleTools>>());
    }

    [Fact]
    public async Task SearchRules_ShouldReturnSerializedResponse()
    {
        var response = new RuleSearchResponse(
            new Paging(1, 100, 1),
            [new RuleDetail("cs:S1234", "Rule Name", "cs", "C#", "MAJOR", "READY", "CODE_SMELL", ["convention"], "<p>Desc</p>")]);

        _client.SearchRulesAsync(Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<string?>(),
                Arg.Any<string?>(), Arg.Any<int?>(), Arg.Any<int?>(), Arg.Any<CancellationToken>())
            .Returns(response);

        var result = await _tools.search_rules(languages: "cs");

        var deserialized = JsonSerializer.Deserialize<RuleSearchResponse>(result);
        deserialized.Should().NotBeNull();
        deserialized!.Rules.Should().HaveCount(1);
        deserialized.Rules[0].Key.Should().Be("cs:S1234");
    }

    [Fact]
    public async Task SearchRules_WhenClientThrows_ShouldPropagateException()
    {
        _client.SearchRulesAsync(Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<string?>(),
                Arg.Any<string?>(), Arg.Any<int?>(), Arg.Any<int?>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Connection failed"));

        var act = () => _tools.search_rules();

        await act.Should().ThrowAsync<HttpRequestException>();
    }
}
