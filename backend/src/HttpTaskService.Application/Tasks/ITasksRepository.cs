namespace HttpTaskService.Application.Tasks;

/// <summary>
/// Interface for tasks repository.
/// </summary>
public interface ITasksRepository
{
    /// <summary>
    /// Creates a new task.
    /// </summary>
    /// <param name="task">The task to create.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created task.</returns>
    Task<Domain.Entities.HttpTask> CreateTaskAsync(
        Domain.Entities.HttpTask task, 
        CancellationToken cancellationToken);
    
    Task<List<Domain.Entities.HttpTask>> GetRetryableTasksAsync(
        int limit,
        int maxRetries,
        int baseRetryDelaySeconds,
        double retryBackoffMultiplier,
        CancellationToken cancellationToken);
    
    Task UpdateTaskAsync(
        Domain.Entities.HttpTask task, 
        CancellationToken cancellationToken);
    
    Task<Domain.Entities.HttpTask?> GetTaskByIdAsync(
        Guid id, 
        CancellationToken cancellationToken);
}