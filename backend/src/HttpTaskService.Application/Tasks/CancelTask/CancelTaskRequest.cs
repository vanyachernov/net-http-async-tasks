namespace HttpTaskService.Application.Tasks.CancelTask;

/// <summary>
/// Request to cancel a task.
/// </summary>
public record CancelTaskRequest(Guid TaskId);
