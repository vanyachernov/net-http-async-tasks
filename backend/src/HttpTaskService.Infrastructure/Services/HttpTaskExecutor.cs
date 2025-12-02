using System.Diagnostics;
using HttpTaskService.Application.Services;
using HttpTaskService.Application.Tasks;
using HttpTaskService.Domain.Entities;
using Microsoft.Extensions.Logging;
using TaskStatus = HttpTaskService.Domain.Shared.TaskStatus;

namespace HttpTaskService.Infrastructure.Services;

/// <summary>
/// Service responsible for executing HTTP tasks.
/// </summary>
public class HttpTaskExecutor : IHttpTaskExecutor
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ITasksRepository _tasksRepository;
    private readonly ILogger<HttpTaskExecutor> _logger;

    public HttpTaskExecutor(
        IHttpClientFactory httpClientFactory,
        ITasksRepository tasksRepository,
        ILogger<HttpTaskExecutor> logger)
    {
        _httpClientFactory = httpClientFactory;
        _tasksRepository = tasksRepository;
        _logger = logger;
    }

    public async Task ExecuteTaskAsync(HttpTask task, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting execution of task with ID {task.Id} for URL {task.Url}...");
        
        task.Status = TaskStatus.Running;
        task.StartedAt = DateTime.UtcNow;
        
        await _tasksRepository.UpdateTaskAsync(task, cancellationToken);

        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30);

            var response = await httpClient.GetAsync(task.Url, cancellationToken);
            
            stopwatch.Stop();
            
            task.StatusCode = (int)response.StatusCode;
            task.ContentLength = response.Content.Headers.ContentLength ?? 0;
            task.DurationMs = stopwatch.ElapsedMilliseconds;
            task.CompletedAt = DateTime.UtcNow;
            task.Status = TaskStatus.Completed;

            _logger.LogInformation($"Task with ID {task.Id} completed successfully. Status: {task.StatusCode}, Duration: {task.DurationMs}ms");
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            
            task.Status = TaskStatus.Failed;
            task.Error = ex.Message;
            task.CompletedAt = DateTime.UtcNow;
            task.DurationMs = stopwatch.ElapsedMilliseconds;

            _logger.LogError(ex, $"Task with ID {task.Id} failed after {task.DurationMs}ms: {ex.Message}");
        }
        
        try
        {
            await _tasksRepository.UpdateTaskAsync(task, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Critical: Failed to save task with ID {task.Id} results to database. Task completed but results lost. Status: \"{task.Status}\"");
        }
    }
}
