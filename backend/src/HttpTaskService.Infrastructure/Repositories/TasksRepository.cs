using HttpTaskService.Application.Tasks;
using HttpTaskService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TaskStatus = HttpTaskService.Domain.Shared.TaskStatus;

namespace HttpTaskService.Infrastructure.Repositories;

public class TasksRepository : ITasksRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TasksRepository(
        ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<HttpTask> CreateTaskAsync(
        HttpTask task, 
        CancellationToken cancellationToken)
    {
        await _dbContext.HttpTasks.AddAsync(task, cancellationToken);
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return task;
    }

    public async Task<List<HttpTask>> GetPendingTasksAsync(
        int limit, 
        CancellationToken cancellationToken)
    {
        return await _dbContext.HttpTasks
            .Where(t => t.Status == TaskStatus.Pending)
            .OrderBy(t => t.CreatedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateTaskAsync(
        HttpTask task, 
        CancellationToken cancellationToken)
    {
        _dbContext.HttpTasks.Update(task);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<HttpTask?> GetTaskByIdAsync(
        Guid id, 
        CancellationToken cancellationToken)
    {
        return await _dbContext.HttpTasks
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }
}