using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using Viamus.Sonarqube.Mcp.Server.Services;

namespace Viamus.Sonarqube.Mcp.Server.Tools;

[McpServerToolType]
public class QualityGateTools(ISonarQubeClient sonarQubeClient, ILogger<QualityGateTools> logger)
{
    [McpServerTool, Description("List all available quality gates in SonarQube. Returns gate definitions including their conditions and thresholds.")]
    public async Task<string> list_quality_gates()
    {
        logger.LogInformation("Listing quality gates");
        var result = await sonarQubeClient.ListQualityGatesAsync();
        return JsonSerializer.Serialize(result);
    }
}
