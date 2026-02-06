using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using Viamus.Sonarqube.Mcp.Server.Services;

namespace Viamus.Sonarqube.Mcp.Server.Tools;

[McpServerToolType]
public class HotspotTools(ISonarQubeClient sonarQubeClient, ILogger<HotspotTools> logger)
{
    [McpServerTool, Description("Search for security hotspots in a SonarQube project. Hotspots are security-sensitive code that requires manual review.")]
    public async Task<string> search_hotspots(
        [Description("The project key to search hotspots in")] string projectKey,
        [Description("Filter by status: TO_REVIEW, REVIEWED")] string? status = null,
        [Description("Page number (1-based). Default: 1")] int? page = null,
        [Description("Page size (1-500). Default: 100")] int? pageSize = null)
    {
        logger.LogInformation("Searching hotspots for project: {ProjectKey}", projectKey);
        var result = await sonarQubeClient.SearchHotspotsAsync(projectKey, status, page, pageSize);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Get detailed information about a specific security hotspot, including its rule, component, changelog, and review status.")]
    public async Task<string> get_hotspot(
        [Description("The hotspot key")] string hotspotKey)
    {
        logger.LogInformation("Getting hotspot details for: {HotspotKey}", hotspotKey);
        var result = await sonarQubeClient.GetHotspotAsync(hotspotKey);
        return JsonSerializer.Serialize(result);
    }
}
