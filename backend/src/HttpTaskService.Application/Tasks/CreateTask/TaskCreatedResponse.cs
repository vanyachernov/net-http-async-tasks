namespace HttpTaskService.Application.Tasks.CreateTask;

public record TaskCreatedResponse(Guid TaskId, string Status = "pending");