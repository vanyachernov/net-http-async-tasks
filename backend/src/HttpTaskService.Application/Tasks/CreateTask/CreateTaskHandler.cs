using HttpTaskService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace HttpTaskService.Application.Tasks.CreateTask;

/// <summary>
/// Handler for creating new tasks.
/// </summary>
public class CreateTaskHandler
{
    private readonly ITasksRepository _tasksRepository;
    private readonly ILogger<CreateTaskHandler> _logger;

    public CreateTaskHandler(
        ITasksRepository tasksRepository,
        ILogger<CreateTaskHandler> logger)
    {
        _tasksRepository = tasksRepository;
        _logger = logger;
    }

    public async Task<TaskCreatedResponse> Handle(
        CreateTaskRequest request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Creating new task for URL {request.Url}");

        try
        {
            var newTask = new HttpTask
            {
                Id = Guid.NewGuid(),
                Url = request.Url,
                Status = Domain.Shared.TaskStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            var createdTask = await _tasksRepository.CreateTaskAsync(
                newTask, 
                cancellationToken);

            _logger.LogInformation($"Task with ID {createdTask.Id} created successfully");

            return new TaskCreatedResponse(
                createdTask.Id, 
                createdTask.Status.ToString().ToLower());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to create task for URL {request.Url}");
            throw; 
        }
    }
}