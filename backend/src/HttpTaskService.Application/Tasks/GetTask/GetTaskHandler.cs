using HttpTaskService.Application.DTOs;
using Microsoft.Extensions.Logging;
using TaskStatus = HttpTaskService.Domain.Shared.TaskStatus;

namespace HttpTaskService.Application.Tasks.GetTask;

/// <summary>
/// Handler for retrieving task status and results.
/// </summary>
public class GetTaskHandler
{
    private readonly ITasksRepository _tasksRepository;
    private readonly ILogger<GetTaskHandler> _logger;

    public GetTaskHandler(
        ITasksRepository tasksRepository,
        ILogger<GetTaskHandler> logger)
    {
        _tasksRepository = tasksRepository;
        _logger = logger;
    }

    public async Task<GetTaskResponse?> Handle(
        GetTaskRequest request, 
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Retrieving task {TaskId}", request.TaskId);

        try
        {
            var task = await _tasksRepository.GetTaskByIdAsync(
                request.TaskId, 
                cancellationToken);

            if (task == null)
            {
                _logger.LogWarning("Task {TaskId} not found", request.TaskId);
                return null;
            }

            _logger.LogDebug("Task {TaskId} found with status {Status}", task.Id, task.Status);

            return task.Status switch
            {
                TaskStatus.Pending => new PendingTaskResponse
                {
                    TaskId = task.Id,
                    Status = "pending"
                },
                
                TaskStatus.Running => new RunningTaskResponse
                {
                    TaskId = task.Id,
                    Status = "running"
                },
                
                TaskStatus.Completed => new CompletedTaskResponse
                {
                    TaskId = task.Id,
                    Status = "completed",
                    ResultDto = new CompletedTaskResultDto
                    {
                        Url = task.Url,
                        StatusCode = task.StatusCode!.Value,
                        Length = task.ContentLength!.Value,
                        DurationMs = task.DurationMs!.Value,
                        CompletedAt = task.CompletedAt!.Value
                    }
                },
                
                TaskStatus.Failed => new FailedTaskResponse
                {
                    TaskId = task.Id,
                    Status = "failed",
                    Error = task.Error ?? "Unknown error"
                },
                
                _ => throw new InvalidOperationException($"Unknown task status: {task.Status}")
            };
        }
        catch (Exception ex) when (ex is not InvalidOperationException)
        {
            _logger.LogError(ex, "Error retrieving task {TaskId}", request.TaskId);
            throw; // Re-throw to let controller handle it
        }
    }
}
