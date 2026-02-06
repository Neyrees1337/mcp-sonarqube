using System.Text.Json.Serialization;

namespace Viamus.Sonarqube.Mcp.Server.Models;

public record Paging(
    [property: JsonPropertyName("pageIndex")] int PageIndex,
    [property: JsonPropertyName("pageSize")] int PageSize,
    [property: JsonPropertyName("total")] int Total
);
