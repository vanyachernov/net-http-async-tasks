using System.Text.Json.Serialization;

namespace HttpTaskService.Application.Tasks.GetTask;

/// <summary>
/// Response for tasks with Failed status.
/// </summary>
public record FailedTaskResponse : GetTaskResponse
{
    [JsonPropertyName("error")]
    public required string Error { get; init; }
}