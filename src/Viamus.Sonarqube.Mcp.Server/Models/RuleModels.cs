using System.Text.Json.Serialization;

namespace Viamus.Sonarqube.Mcp.Server.Models;

public record RuleSearchResponse(
    [property: JsonPropertyName("paging")] Paging Paging,
    [property: JsonPropertyName("rules")] List<RuleDetail> Rules
);

public record RuleDetail(
    [property: JsonPropertyName("key")] string Key,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("lang")] string? Lang,
    [property: JsonPropertyName("langName")] string? LangName,
    [property: JsonPropertyName("severity")] string? Severity,
    [property: JsonPropertyName("status")] string? Status,
    [property: JsonPropertyName("type")] string? Type,
    [property: JsonPropertyName("tags")] List<string>? Tags,
    [property: JsonPropertyName("htmlDesc")] string? HtmlDesc
);
