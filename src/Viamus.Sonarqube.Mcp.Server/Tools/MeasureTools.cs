using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using Viamus.Sonarqube.Mcp.Server.Services;

namespace Viamus.Sonarqube.Mcp.Server.Tools;

[McpServerToolType]
public class MeasureTools(ISonarQubeClient sonarQubeClient, ILogger<MeasureTools> logger)
{
    [McpServerTool, Description("Get measures/metrics for a SonarQube component. Retrieves values for specified metrics like coverage, bugs, vulnerabilities, code_smells, ncloc, etc.")]
    public async Task<string> get_measures(
        [Description("The component key (usually the project key)")] string component,
        [Description("Comma-separated list of metric keys (e.g., 'coverage,bugs,vulnerabilities,code_smells,ncloc,duplicated_lines_density')")] string metricKeys)
    {
        logger.LogInformation("Getting measures for component: {Component}, metrics: {MetricKeys}", component, metricKeys);
        var result = await sonarQubeClient.GetMeasuresAsync(component, metricKeys);
        return JsonSerializer.Serialize(result);
    }
}
