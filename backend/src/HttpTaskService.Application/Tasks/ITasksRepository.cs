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
    public Task<Domain.Entities.HttpTask> CreateTaskAsync(
        Domain.Entities.HttpTask task, 
        CancellationToken cancellationToken);
    
    /// <summary>
    /// Gets a list of pending tasks.
    /// </summary>
    /// <param name="limit">The maximum number of tasks to return.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of pending tasks.</returns>
    public Task<List<Domain.Entities.HttpTask>> GetPendingTasksAsync(
        int limit, 
        CancellationToken cancellationToken);
    
    /// <summary>
    /// Updates an existing task.
    /// </summary>
    /// <param name="task">The task to update.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public Task UpdateTaskAsync(
        Domain.Entities.HttpTask task, 
        CancellationToken cancellationToken);
    
    /// <summary>
    /// Gets a task by ID.
    /// </summary>
    /// <param name="id">The ID of the task to get.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task with the specified ID, or null if not found.</returns>
    public Task<Domain.Entities.HttpTask?> GetTaskByIdAsync(
        Guid id, 
        CancellationToken cancellationToken);
}