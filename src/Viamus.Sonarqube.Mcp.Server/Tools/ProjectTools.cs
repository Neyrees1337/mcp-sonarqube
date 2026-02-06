using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using Viamus.Sonarqube.Mcp.Server.Services;

namespace Viamus.Sonarqube.Mcp.Server.Tools;

[McpServerToolType]
public class ProjectTools(ISonarQubeClient sonarQubeClient, ILogger<ProjectTools> logger)
{
    [McpServerTool, Description("Search for SonarQube projects. Returns project keys, names, and last analysis dates.")]
    public async Task<string> search_projects(
        [Description("Optional text query to filter projects by name or key")] string? query = null,
        [Description("Page number (1-based). Default: 1")] int? page = null,
        [Description("Page size (1-500). Default: 100")] int? pageSize = null)
    {
        logger.LogInformation("Searching projects with query: {Query}", query);
        var result = await sonarQubeClient.SearchProjectsAsync(query, page, pageSize);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Get the quality gate status and key measures for a project. Combines quality gate status with important metrics like coverage, bugs, vulnerabilities, and code smells.")]
    public async Task<string> get_project_status(
        [Description("The project key")] string projectKey)
    {
        logger.LogInformation("Getting project status for: {ProjectKey}", projectKey);

        var statusTask = sonarQubeClient.GetQualityGateProjectStatusAsync(projectKey);
        var measuresTask = sonarQubeClient.GetMeasuresAsync(projectKey,
            "coverage,bugs,vulnerabilities,code_smells,duplicated_lines_density,ncloc,security_hotspots");

        await Task.WhenAll(statusTask, measuresTask);

        var combined = new
        {
            qualityGate = statusTask.Result,
            measures = measuresTask.Result
        };

        return JsonSerializer.Serialize(combined);
    }
}
