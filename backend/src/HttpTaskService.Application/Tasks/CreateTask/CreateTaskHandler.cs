using HttpTaskService.Domain.Entities;

namespace HttpTaskService.Application.Tasks.CreateTask;

public class CreateTaskHandler(ITasksRepository tasksRepository)
{
    public async Task<TaskCreatedResponse> Handle(
        CreateTaskRequest request, 
        CancellationToken cancellationToken)
    {
        var newTask = new HttpTask
        {
            Id = Guid.NewGuid(),
            Url = request.Url,
            Status = Domain.Shared.TaskStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        var createdTask = await tasksRepository.CreateTaskAsync(
            newTask, 
            cancellationToken);

        return new TaskCreatedResponse(
            createdTask.Id, 
            createdTask.Status.ToString().ToLower());
    }
}