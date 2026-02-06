using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using Viamus.Sonarqube.Mcp.Server.Services;

namespace Viamus.Sonarqube.Mcp.Server.Tools;

[McpServerToolType]
public class SystemTools(ISonarQubeClient sonarQubeClient, ILogger<SystemTools> logger)
{
    [McpServerTool, Description("Get the health status of the SonarQube instance. Returns GREEN, YELLOW, or RED with causes if unhealthy.")]
    public async Task<string> get_health()
    {
        logger.LogInformation("Getting system health status");
        var result = await sonarQubeClient.GetSystemHealthAsync();
        return JsonSerializer.Serialize(result);
    }
}
