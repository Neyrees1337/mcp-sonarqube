using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using Viamus.Sonarqube.Mcp.Server.Services;

namespace Viamus.Sonarqube.Mcp.Server.Tools;

[McpServerToolType]
public class RuleTools(ISonarQubeClient sonarQubeClient, ILogger<RuleTools> logger)
{
    [McpServerTool, Description("Search for coding rules in SonarQube. Filter by language, severity, or tags. Returns rule definitions and descriptions.")]
    public async Task<string> search_rules(
        [Description("Comma-separated language keys (e.g., 'cs,java,js')")] string? languages = null,
        [Description("Comma-separated severities: INFO, MINOR, MAJOR, CRITICAL, BLOCKER")] string? severities = null,
        [Description("Comma-separated tags to filter by")] string? tags = null,
        [Description("Text query to search rules by name or description")] string? query = null,
        [Description("Page number (1-based). Default: 1")] int? page = null,
        [Description("Page size (1-500). Default: 100")] int? pageSize = null)
    {
        logger.LogInformation("Searching rules with query: {Query}", query);
        var result = await sonarQubeClient.SearchRulesAsync(languages, severities, tags, query, page, pageSize);
        return JsonSerializer.Serialize(result);
    }
}
