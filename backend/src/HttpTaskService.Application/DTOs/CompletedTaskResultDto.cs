using System.Text.Json.Serialization;

namespace HttpTaskService.Application.DTOs;

/// <summary>
/// Result data for completed tasks.
/// </summary>
public record CompletedTaskResultDto
{
    [JsonPropertyName("url")]
    public required string Url { get; init; }
    
    [JsonPropertyName("status_code")]
    public required int StatusCode { get; init; }
    
    [JsonPropertyName("length")]
    public required long Length { get; init; }
    
    [JsonPropertyName("duration_ms")]
    public required long DurationMs { get; init; }
    
    [JsonPropertyName("completed_at")]
    public required DateTime CompletedAt { get; init; }
}