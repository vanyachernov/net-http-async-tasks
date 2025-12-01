namespace HttpTaskService.Application.Tasks;

public interface ITasksRepository
{
    public Task<Domain.Entities.HttpTask> CreateTaskAsync(
        Domain.Entities.HttpTask task, 
        CancellationToken cancellationToken);
}