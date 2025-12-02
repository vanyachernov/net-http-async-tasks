namespace HttpTaskService.Application.Tasks.CreateTask;

/// <summary>
/// Response for task creation.
/// </summary>
public record TaskCreatedResponse(Guid TaskId, string Status = "pending");