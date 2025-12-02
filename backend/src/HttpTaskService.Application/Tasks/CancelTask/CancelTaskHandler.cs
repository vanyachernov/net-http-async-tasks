using Microsoft.Extensions.Logging;
using TaskStatus = HttpTaskService.Domain.Shared.TaskStatus;

namespace HttpTaskService.Application.Tasks.CancelTask;

/// <summary>
/// Handler for cancelling tasks.
/// </summary>
public class CancelTaskHandler
{
    private readonly ITasksRepository _tasksRepository;
    private readonly ILogger<CancelTaskHandler> _logger;

    public CancelTaskHandler(
        ITasksRepository tasksRepository,
        ILogger<CancelTaskHandler> logger)
    {
        _tasksRepository = tasksRepository;
        _logger = logger;
    }

    public async Task<CancelTaskResponse?> Handle(
        CancelTaskRequest request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Attempting to cancel task with ID {request.TaskId}");

        try
        {
            var task = await _tasksRepository.GetTaskByIdAsync(
                request.TaskId, 
                cancellationToken);

            if (task == null)
            {
                _logger.LogWarning("Task {TaskId} not found for cancellation", request.TaskId);
                return null;
            }

            if (task.Status != TaskStatus.Pending && task.Status != TaskStatus.Running)
            {
                _logger.LogWarning("Cannot cancel task {TaskId} with status {Status}", task.Id, task.Status);
                throw new InvalidOperationException($"Cannot cancel task with status '{task.Status.ToString().ToLower()}'");
            }

            task.Status = TaskStatus.Cancelled;
            task.CancelledAt = DateTime.UtcNow;

            await _tasksRepository.UpdateTaskAsync(task, cancellationToken);

            _logger.LogInformation("Task {TaskId} cancelled successfully", task.Id);

            return new CancelTaskResponse
            {
                TaskId = task.Id,
                Status = "cancelled"
            };
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error cancelling task with ID {request.TaskId}");
            throw;
        }
    }
}
