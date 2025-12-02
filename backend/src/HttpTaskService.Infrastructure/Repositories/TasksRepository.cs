using HttpTaskService.Application.Tasks;
using HttpTaskService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskStatus = HttpTaskService.Domain.Shared.TaskStatus;

namespace HttpTaskService.Infrastructure.Repositories;

public class TasksRepository : ITasksRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<TasksRepository> _logger;

    public TasksRepository(
        ApplicationDbContext dbContext,
        ILogger<TasksRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<HttpTask> CreateTaskAsync(
        HttpTask task, 
        CancellationToken cancellationToken)
    {
        await _dbContext.HttpTasks.AddAsync(task, cancellationToken);
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Created task with ID {task.Id} for URL {task.Url}");

        return task;
    }

    public async Task<List<HttpTask>> GetPendingTasksAsync(
        int limit, 
        CancellationToken cancellationToken)
    {
        var tasks = await _dbContext.HttpTasks
            .Where(t => t.Status == TaskStatus.Pending)
            .OrderBy(t => t.CreatedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);

        if (tasks.Count > 0)
        {
            _logger.LogDebug($"Retrieved {tasks.Count} pending tasks from database.");
        }

        return tasks;
    }

    public async Task UpdateTaskAsync(
        HttpTask task, 
        CancellationToken cancellationToken)
    {
        _dbContext.HttpTasks.Update(task);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogDebug($"Updated task with ID {task.Id} with status \"{task.Status}\".");
    }

    public async Task<HttpTask?> GetTaskByIdAsync(
        Guid id, 
        CancellationToken cancellationToken)
    {
        return await _dbContext.HttpTasks
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }
}