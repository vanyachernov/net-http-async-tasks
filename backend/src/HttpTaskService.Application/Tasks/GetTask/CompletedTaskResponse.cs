using System.Text.Json.Serialization;
using HttpTaskService.Application.DTOs;

namespace HttpTaskService.Application.Tasks.GetTask;

/// <summary>
/// Response for tasks with Completed status.
/// </summary>
public record CompletedTaskResponse : GetTaskResponse
{
    [JsonPropertyName("result")]
    public required CompletedTaskResultDto ResultDto { get; init; }
}