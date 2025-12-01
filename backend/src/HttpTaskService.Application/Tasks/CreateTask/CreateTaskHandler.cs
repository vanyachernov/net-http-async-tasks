using HttpTaskService.Domain.Entities;

namespace HttpTaskService.Application.Tasks.CreateTask;

public class CreateVolunteerHandler(ITasksRepository tasksRepository)
{
    public async Task<TaskCreatedResponse> Handle(
        CreateTaskRequest request, 
        CancellationToken cancellationToken)
    {
        var newTask = new HttpTask
        {
            Url = request.Url
        };

        var createdTask = await tasksRepository.CreateTaskAsync(
            newTask, 
            cancellationToken);

        return new TaskCreatedResponse(createdTask.Id);
    }
}