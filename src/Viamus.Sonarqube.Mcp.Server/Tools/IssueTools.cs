using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using Viamus.Sonarqube.Mcp.Server.Services;

namespace Viamus.Sonarqube.Mcp.Server.Tools;

[McpServerToolType]
public class IssueTools(ISonarQubeClient sonarQubeClient, ILogger<IssueTools> logger)
{
    [McpServerTool, Description("Search for issues (bugs, vulnerabilities, code smells) in SonarQube. Filter by project, severity, status, or type.")]
    public async Task<string> search_issues(
        [Description("Project key to filter issues")] string? projectKey = null,
        [Description("Comma-separated severities: INFO, MINOR, MAJOR, CRITICAL, BLOCKER")] string? severities = null,
        [Description("Comma-separated statuses: OPEN, CONFIRMED, REOPENED, RESOLVED, CLOSED")] string? statuses = null,
        [Description("Comma-separated types: CODE_SMELL, BUG, VULNERABILITY")] string? types = null,
        [Description("Comma-separated tags to filter by")] string? tags = null,
        [Description("Page number (1-based). Default: 1")] int? page = null,
        [Description("Page size (1-500). Default: 100")] int? pageSize = null)
    {
        logger.LogInformation("Searching issues for project: {ProjectKey}", projectKey);
        var result = await sonarQubeClient.SearchIssuesAsync(projectKey, severities, statuses, types, tags, page, pageSize);
        return JsonSerializer.Serialize(result);
    }
}
