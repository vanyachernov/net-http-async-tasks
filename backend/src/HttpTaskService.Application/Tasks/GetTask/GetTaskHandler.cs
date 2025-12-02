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
        _logger.LogDebug($"Retrieving task with ID {request.TaskId}.");

        try
        {
            var task = await _tasksRepository.GetTaskByIdAsync(
                request.TaskId, 
                cancellationToken);

            if (task == null)
            {
                _logger.LogWarning($"Task {request.TaskId} not found.");
                return null;
            }

            _logger.LogDebug($"Task with ID {task.Id} found with status \"{task.Status}\".");

            return task.Status switch
            {
                TaskStatus.Pending => new PendingTaskResponse
                {
                    TaskId = task.Id,
                    Status = nameof(TaskStatus.Pending)
                },
                
                TaskStatus.Running => new RunningTaskResponse
                {
                    TaskId = task.Id,
                    Status = nameof(TaskStatus.Running)
                },
                
                TaskStatus.Completed => new CompletedTaskResponse
                {
                    TaskId = task.Id,
                    Status = nameof(TaskStatus.Completed),
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
                    Status = nameof(TaskStatus.Failed),
                    Error = task.Error ?? "Unknown error"
                },
                
                TaskStatus.Cancelled => new CancelledTaskResponse
                {
                    TaskId = task.Id,
                    Status = nameof(TaskStatus.Cancelled),
                    CancelledAt = task.CancelledAt!.Value
                },
                
                _ => throw new InvalidOperationException($"Unknown task status: {task.Status}")
            };
        }
        catch (Exception ex) when (ex is not InvalidOperationException)
        {
            _logger.LogError(ex, $"Error retrieving task with ID {request.TaskId}");
            throw;
        }
    }
}
