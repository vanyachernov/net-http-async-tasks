using HttpTaskService.Application.Services;
using HttpTaskService.Application.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TaskStatus = HttpTaskService.Domain.Shared.TaskStatus;

namespace HttpTaskService.Infrastructure.BackgroundServices;

/// <summary>
/// Background service that periodically processes pending HTTP tasks.
/// </summary>
public class HttpTaskBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<HttpTaskBackgroundService> _logger;
    private readonly TimeSpan _pollingInterval = TimeSpan.FromSeconds(5);
    private const int MaxTasksPerBatch = 10;
    
    // Retry
    private const int MaxRetries = 3;
    private const int BaseRetryDelaySeconds = 5;
    private const double RetryBackoffMultiplier = 2.0;

    public HttpTaskBackgroundService(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<HttpTaskBackgroundService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("HTTP Task Background Service started...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingTasksAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing pending tasks");
            }

            await Task.Delay(_pollingInterval, stoppingToken);
        }

        _logger.LogInformation("HTTP Task Background Service stopped.");
    }

    private async Task ProcessPendingTasksAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        
        var tasksRepository = scope.ServiceProvider.GetRequiredService<ITasksRepository>();
        var taskExecutor = scope.ServiceProvider.GetRequiredService<IHttpTaskExecutor>();

        var retryableTasks = await tasksRepository.GetRetryableTasksAsync(
            MaxTasksPerBatch,
            MaxRetries,
            BaseRetryDelaySeconds,
            RetryBackoffMultiplier,
            cancellationToken);

        if (retryableTasks.Count > 0)
        {
            _logger.LogInformation($"Found {retryableTasks.Count} retryable tasks to process");

            foreach (var task in retryableTasks)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                if (task.Status == TaskStatus.Cancelled)
                {
                    _logger.LogDebug($"Skipping cancelled task with ID {task.Id}");
                    continue;
                }

                try
                {
                    await taskExecutor.ExecuteTaskAsync(task, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Unexpected error while executing task with ID {task.Id}");
                }
            }
        }
    }
}
