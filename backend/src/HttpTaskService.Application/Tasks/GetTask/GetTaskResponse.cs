using System.Text.Json.Serialization;

namespace HttpTaskService.Application.Tasks.GetTask;

/// <summary>
/// Base response for task status queries.
/// </summary>
public abstract record GetTaskResponse
{
    [JsonPropertyName("task_id")]
    public required Guid TaskId { get; init; }
    
    [JsonPropertyName("status")]
    public required string Status { get; init; }
}