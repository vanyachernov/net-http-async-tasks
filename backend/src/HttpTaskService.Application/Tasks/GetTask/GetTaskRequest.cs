namespace HttpTaskService.Application.Tasks.GetTask;

/// <summary>
/// Request to get task by its ID.
/// </summary>
public record GetTaskRequest(Guid TaskId);
