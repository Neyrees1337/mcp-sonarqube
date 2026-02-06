using System.Text.Json.Serialization;

namespace Viamus.Sonarqube.Mcp.Server.Models;

public record HotspotSearchResponse(
    [property: JsonPropertyName("paging")] Paging Paging,
    [property: JsonPropertyName("hotspots")] List<Hotspot> Hotspots,
    [property: JsonPropertyName("components")] List<HotspotComponent>? Components
);

public record Hotspot(
    [property: JsonPropertyName("key")] string Key,
    [property: JsonPropertyName("component")] string Component,
    [property: JsonPropertyName("project")] string Project,
    [property: JsonPropertyName("securityCategory")] string? SecurityCategory,
    [property: JsonPropertyName("vulnerabilityProbability")] string? VulnerabilityProbability,
    [property: JsonPropertyName("status")] string? Status,
    [property: JsonPropertyName("line")] int? Line,
    [property: JsonPropertyName("message")] string? Message,
    [property: JsonPropertyName("author")] string? Author,
    [property: JsonPropertyName("creationDate")] string? CreationDate,
    [property: JsonPropertyName("updateDate")] string? UpdateDate
);

public record HotspotComponent(
    [property: JsonPropertyName("key")] string Key,
    [property: JsonPropertyName("qualifier")] string? Qualifier,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("path")] string? Path
);

public record HotspotDetailResponse(
    [property: JsonPropertyName("key")] string Key,
    [property: JsonPropertyName("component")] HotspotDetailComponent Component,
    [property: JsonPropertyName("project")] HotspotDetailProject Project,
    [property: JsonPropertyName("rule")] HotspotDetailRule Rule,
    [property: JsonPropertyName("status")] string? Status,
    [property: JsonPropertyName("message")] string? Message,
    [property: JsonPropertyName("line")] int? Line,
    [property: JsonPropertyName("securityCategory")] string? SecurityCategory,
    [property: JsonPropertyName("vulnerabilityProbability")] string? VulnerabilityProbability,
    [property: JsonPropertyName("author")] string? Author,
    [property: JsonPropertyName("creationDate")] string? CreationDate,
    [property: JsonPropertyName("updateDate")] string? UpdateDate,
    [property: JsonPropertyName("changelog")] List<HotspotChangelog>? Changelog
);

public record HotspotDetailComponent(
    [property: JsonPropertyName("key")] string Key,
    [property: JsonPropertyName("qualifier")] string? Qualifier,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("path")] string? Path
);

public record HotspotDetailProject(
    [property: JsonPropertyName("key")] string Key,
    [property: JsonPropertyName("name")] string? Name
);

public record HotspotDetailRule(
    [property: JsonPropertyName("key")] string Key,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("securityCategory")] string? SecurityCategory,
    [property: JsonPropertyName("vulnerabilityProbability")] string? VulnerabilityProbability
);

public record HotspotChangelog(
    [property: JsonPropertyName("user")] string? User,
    [property: JsonPropertyName("creationDate")] string? CreationDate,
    [property: JsonPropertyName("diffs")] List<HotspotDiff>? Diffs
);

public record HotspotDiff(
    [property: JsonPropertyName("key")] string Key,
    [property: JsonPropertyName("newValue")] string? NewValue,
    [property: JsonPropertyName("oldValue")] string? OldValue
);
