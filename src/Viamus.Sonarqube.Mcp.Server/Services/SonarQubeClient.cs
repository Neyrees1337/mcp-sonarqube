using System.Net.Http.Json;
using Viamus.Sonarqube.Mcp.Server.Models;

namespace Viamus.Sonarqube.Mcp.Server.Services;

public class SonarQubeClient(HttpClient httpClient) : ISonarQubeClient
{
    public async Task<ProjectSearchResponse> SearchProjectsAsync(
        string? query, int? page, int? pageSize, CancellationToken cancellationToken)
    {
        var parameters = new List<string>();
        if (!string.IsNullOrWhiteSpace(query)) parameters.Add($"q={Uri.EscapeDataString(query)}");
        if (page.HasValue) parameters.Add($"p={page.Value}");
        if (pageSize.HasValue) parameters.Add($"ps={pageSize.Value}");

        var url = BuildUrl("/api/projects/search", parameters);
        var response = await httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ProjectSearchResponse>(cancellationToken))!;
    }

    public async Task<IssueSearchResponse> SearchIssuesAsync(
        string? projectKey, string? severities, string? statuses,
        string? types, string? tags, int? page, int? pageSize,
        CancellationToken cancellationToken)
    {
        var parameters = new List<string>();
        if (!string.IsNullOrWhiteSpace(projectKey)) parameters.Add($"projects={Uri.EscapeDataString(projectKey)}");
        if (!string.IsNullOrWhiteSpace(severities)) parameters.Add($"severities={Uri.EscapeDataString(severities)}");
        if (!string.IsNullOrWhiteSpace(statuses)) parameters.Add($"statuses={Uri.EscapeDataString(statuses)}");
        if (!string.IsNullOrWhiteSpace(types)) parameters.Add($"types={Uri.EscapeDataString(types)}");
        if (!string.IsNullOrWhiteSpace(tags)) parameters.Add($"tags={Uri.EscapeDataString(tags)}");
        if (page.HasValue) parameters.Add($"p={page.Value}");
        if (pageSize.HasValue) parameters.Add($"ps={pageSize.Value}");

        var url = BuildUrl("/api/issues/search", parameters);
        var response = await httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<IssueSearchResponse>(cancellationToken))!;
    }

    public async Task<QualityGateProjectStatusResponse> GetQualityGateProjectStatusAsync(
        string projectKey, CancellationToken cancellationToken)
    {
        var url = BuildUrl("/api/qualitygates/project_status", [$"projectKey={Uri.EscapeDataString(projectKey)}"]);
        var response = await httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<QualityGateProjectStatusResponse>(cancellationToken))!;
    }

    public async Task<MeasureComponentResponse> GetMeasuresAsync(
        string component, string metricKeys, CancellationToken cancellationToken)
    {
        var parameters = new List<string>
        {
            $"component={Uri.EscapeDataString(component)}",
            $"metricKeys={Uri.EscapeDataString(metricKeys)}"
        };

        var url = BuildUrl("/api/measures/component", parameters);
        var response = await httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<MeasureComponentResponse>(cancellationToken))!;
    }

    public async Task<HotspotSearchResponse> SearchHotspotsAsync(
        string projectKey, string? status, int? page, int? pageSize,
        CancellationToken cancellationToken)
    {
        var parameters = new List<string> { $"projectKey={Uri.EscapeDataString(projectKey)}" };
        if (!string.IsNullOrWhiteSpace(status)) parameters.Add($"status={Uri.EscapeDataString(status)}");
        if (page.HasValue) parameters.Add($"p={page.Value}");
        if (pageSize.HasValue) parameters.Add($"ps={pageSize.Value}");

        var url = BuildUrl("/api/hotspots/search", parameters);
        var response = await httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<HotspotSearchResponse>(cancellationToken))!;
    }

    public async Task<HotspotDetailResponse> GetHotspotAsync(
        string hotspotKey, CancellationToken cancellationToken)
    {
        var url = BuildUrl("/api/hotspots/show", [$"hotspot={Uri.EscapeDataString(hotspotKey)}"]);
        var response = await httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<HotspotDetailResponse>(cancellationToken))!;
    }

    public async Task<QualityGateListResponse> ListQualityGatesAsync(CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync("/api/qualitygates/list", cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<QualityGateListResponse>(cancellationToken))!;
    }

    public async Task<SystemHealthResponse> GetSystemHealthAsync(CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync("/api/system/health", cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<SystemHealthResponse>(cancellationToken))!;
    }

    public async Task<RuleSearchResponse> SearchRulesAsync(
        string? languages, string? severities, string? tags,
        string? query, int? page, int? pageSize,
        CancellationToken cancellationToken)
    {
        var parameters = new List<string>();
        if (!string.IsNullOrWhiteSpace(languages)) parameters.Add($"languages={Uri.EscapeDataString(languages)}");
        if (!string.IsNullOrWhiteSpace(severities)) parameters.Add($"severities={Uri.EscapeDataString(severities)}");
        if (!string.IsNullOrWhiteSpace(tags)) parameters.Add($"tags={Uri.EscapeDataString(tags)}");
        if (!string.IsNullOrWhiteSpace(query)) parameters.Add($"q={Uri.EscapeDataString(query)}");
        if (page.HasValue) parameters.Add($"p={page.Value}");
        if (pageSize.HasValue) parameters.Add($"ps={pageSize.Value}");

        var url = BuildUrl("/api/rules/search", parameters);
        var response = await httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<RuleSearchResponse>(cancellationToken))!;
    }

    private static string BuildUrl(string path, List<string> parameters)
    {
        return parameters.Count > 0 ? $"{path}?{string.Join("&", parameters)}" : path;
    }
}
