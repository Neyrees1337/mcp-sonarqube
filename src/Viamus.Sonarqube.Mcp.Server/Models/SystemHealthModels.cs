using System.Text.Json.Serialization;

namespace Viamus.Sonarqube.Mcp.Server.Models;

public record SystemHealthResponse(
    [property: JsonPropertyName("health")] string Health,
    [property: JsonPropertyName("causes")] List<HealthCause>? Causes
);

public record HealthCause(
    [property: JsonPropertyName("message")] string Message
);
