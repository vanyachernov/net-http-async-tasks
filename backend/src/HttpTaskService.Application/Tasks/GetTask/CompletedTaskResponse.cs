using System.Text.Json.Serialization;

namespace HttpTaskService.Application.Tasks.GetTask;

/// <summary>
/// Response for tasks with Completed status.
/// </summary>
public record CompletedTaskResponse : GetTaskResponse
{
    [JsonPropertyName("result")]
    public required CompletedTaskResult Result { get; init; }
}