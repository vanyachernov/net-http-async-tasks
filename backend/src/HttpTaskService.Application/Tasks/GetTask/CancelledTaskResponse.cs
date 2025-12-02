using System.Text.Json.Serialization;

namespace HttpTaskService.Application.Tasks.GetTask;

/// <summary>
/// Response for tasks with Cancelled status.
/// </summary>
public record CancelledTaskResponse : GetTaskResponse
{
    [JsonPropertyName("cancelled_at")]
    public required DateTime CancelledAt { get; init; }
}
