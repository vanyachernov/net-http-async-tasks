namespace HttpTaskService.Application.Tasks;

public interface ITasksRepository
{
    public Task<Domain.Entities.HttpTask> CreateTaskAsync(
        Domain.Entities.HttpTask task, 
        CancellationToken cancellationToken);
    
    public Task<List<Domain.Entities.HttpTask>> GetPendingTasksAsync(
        int limit, 
        CancellationToken cancellationToken);
    
    public Task UpdateTaskAsync(
        Domain.Entities.HttpTask task, 
        CancellationToken cancellationToken);
    
    public Task<Domain.Entities.HttpTask?> GetTaskByIdAsync(
        Guid id, 
        CancellationToken cancellationToken);
}