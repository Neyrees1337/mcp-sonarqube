using System.Text.Json.Serialization;

namespace Viamus.Sonarqube.Mcp.Server.Models;

public record IssueSearchResponse(
    [property: JsonPropertyName("paging")] Paging Paging,
    [property: JsonPropertyName("issues")] List<Issue> Issues,
    [property: JsonPropertyName("components")] List<IssueComponent>? Components,
    [property: JsonPropertyName("rules")] List<IssueRule>? Rules
);

public record Issue(
    [property: JsonPropertyName("key")] string Key,
    [property: JsonPropertyName("rule")] string Rule,
    [property: JsonPropertyName("severity")] string? Severity,
    [property: JsonPropertyName("component")] string Component,
    [property: JsonPropertyName("project")] string Project,
    [property: JsonPropertyName("line")] int? Line,
    [property: JsonPropertyName("textRange")] TextRange? TextRange,
    [property: JsonPropertyName("status")] string? Status,
    [property: JsonPropertyName("message")] string? Message,
    [property: JsonPropertyName("effort")] string? Effort,
    [property: JsonPropertyName("debt")] string? Debt,
    [property: JsonPropertyName("author")] string? Author,
    [property: JsonPropertyName("tags")] List<string>? Tags,
    [property: JsonPropertyName("type")] string? Type,
    [property: JsonPropertyName("creationDate")] string? CreationDate,
    [property: JsonPropertyName("updateDate")] string? UpdateDate
);

public record TextRange(
    [property: JsonPropertyName("startLine")] int StartLine,
    [property: JsonPropertyName("endLine")] int EndLine,
    [property: JsonPropertyName("startOffset")] int StartOffset,
    [property: JsonPropertyName("endOffset")] int EndOffset
);

public record IssueComponent(
    [property: JsonPropertyName("key")] string Key,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("qualifier")] string? Qualifier,
    [property: JsonPropertyName("path")] string? Path
);

public record IssueRule(
    [property: JsonPropertyName("key")] string Key,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("lang")] string? Lang,
    [property: JsonPropertyName("status")] string? Status
);
