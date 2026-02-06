using System.Text.Json.Serialization;

namespace Viamus.Sonarqube.Mcp.Server.Models;

public record ProjectSearchResponse(
    [property: JsonPropertyName("paging")] Paging Paging,
    [property: JsonPropertyName("components")] List<Project> Components
);

public record Project(
    [property: JsonPropertyName("key")] string Key,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("qualifier")] string? Qualifier,
    [property: JsonPropertyName("visibility")] string? Visibility,
    [property: JsonPropertyName("lastAnalysisDate")] string? LastAnalysisDate,
    [property: JsonPropertyName("revision")] string? Revision
);
