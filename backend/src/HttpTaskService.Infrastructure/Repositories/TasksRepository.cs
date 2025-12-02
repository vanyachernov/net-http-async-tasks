using HttpTaskService.Application.Tasks;
using HttpTaskService.Domain.Entities;

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
}