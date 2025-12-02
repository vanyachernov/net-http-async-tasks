using HttpTaskService.Domain.Entities;

namespace HttpTaskService.Application.Services;

/// <summary>
/// Service responsible for executing HTTP tasks.
/// </summary>
public interface IHttpTaskExecutor
{
    /// <summary>
    /// Executes an HTTP task by making a request to the specified URL
    /// and updating the task with the results.
    /// </summary>
    /// <param name="task">The task to execute</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task ExecuteTaskAsync(HttpTask task, CancellationToken cancellationToken);
}
