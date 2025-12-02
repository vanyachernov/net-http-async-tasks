using TaskStatus = HttpTaskService.Domain.Shared.TaskStatus;

namespace HttpTaskService.Application.Tasks.GetTask;

/// <summary>
/// Handler for retrieving task status and results.
/// </summary>
public class GetTaskHandler
{
    private readonly ITasksRepository _tasksRepository;

    public GetTaskHandler(ITasksRepository tasksRepository)
    {
        _tasksRepository = tasksRepository;
    }

    public async Task<GetTaskResponse?> Handle(
        GetTaskRequest request, 
        CancellationToken cancellationToken)
    {
        var task = await _tasksRepository.GetTaskByIdAsync(
            request.TaskId, 
            cancellationToken);

        if (task == null)
        {
            return null;
        }

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
                Result = new CompletedTaskResult
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
}
