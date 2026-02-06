using System.Text.Json.Serialization;

namespace Viamus.Sonarqube.Mcp.Server.Models;

public record MeasureComponentResponse(
    [property: JsonPropertyName("component")] MeasureComponent Component
);

public record MeasureComponent(
    [property: JsonPropertyName("key")] string Key,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("qualifier")] string? Qualifier,
    [property: JsonPropertyName("measures")] List<Measure>? Measures
);

public record Measure(
    [property: JsonPropertyName("metric")] string Metric,
    [property: JsonPropertyName("value")] string? Value,
    [property: JsonPropertyName("period")] MeasurePeriod? Period
);

public record MeasurePeriod(
    [property: JsonPropertyName("value")] string? Value,
    [property: JsonPropertyName("bestValue")] bool? BestValue
);
