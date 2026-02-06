using System.Text.Json.Serialization;

namespace Viamus.Sonarqube.Mcp.Server.Models;

public record QualityGateProjectStatusResponse(
    [property: JsonPropertyName("projectStatus")] ProjectQualityGateStatus ProjectStatus
);

public record ProjectQualityGateStatus(
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("conditions")] List<QualityGateCondition>? Conditions
);

public record QualityGateCondition(
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("metricKey")] string MetricKey,
    [property: JsonPropertyName("comparator")] string? Comparator,
    [property: JsonPropertyName("errorThreshold")] string? ErrorThreshold,
    [property: JsonPropertyName("actualValue")] string? ActualValue
);

public record QualityGateListResponse(
    [property: JsonPropertyName("qualitygates")] List<QualityGate> QualityGates,
    [property: JsonPropertyName("default")] long? Default
);

public record QualityGate(
    [property: JsonPropertyName("id")] long Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("isDefault")] bool IsDefault,
    [property: JsonPropertyName("isBuiltIn")] bool IsBuiltIn,
    [property: JsonPropertyName("conditions")] List<QualityGateDefinitionCondition>? Conditions
);

public record QualityGateDefinitionCondition(
    [property: JsonPropertyName("id")] long Id,
    [property: JsonPropertyName("metric")] string Metric,
    [property: JsonPropertyName("op")] string? Op,
    [property: JsonPropertyName("error")] string? Error
);
