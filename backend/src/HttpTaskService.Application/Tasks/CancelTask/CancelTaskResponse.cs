using System.Text.Json.Serialization;

namespace HttpTaskService.Application.Tasks.CancelTask;

/// <summary>
/// Response after cancelling a task.
/// </summary>
public record CancelTaskResponse
{
    [JsonPropertyName("task_id")]
    public required Guid TaskId { get; init; }
    
    [JsonPropertyName("status")]
    public required string Status { get; init; }
}
