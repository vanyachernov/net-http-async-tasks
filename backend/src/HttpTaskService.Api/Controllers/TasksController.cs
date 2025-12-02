using HttpTaskService.Application.Tasks.CancelTask;
using HttpTaskService.Application.Tasks.CreateTask;
using HttpTaskService.Application.Tasks.GetTask;
using Microsoft.AspNetCore.Mvc;

namespace HttpTaskService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController(
    CreateTaskHandler createTaskHandler,
    GetTaskHandler getTaskHandler,
    CancelTaskHandler cancelTaskHandler) : ControllerBase
{
    /// <summary>
    /// Creates a new HTTP task to be executed asynchronously.
    /// </summary>
    /// <param name="request">The task creation request containing the URL</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created task with its ID and status</returns>
    [HttpPost]
    [ProducesResponseType(typeof(TaskCreatedResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTask(
        [FromBody] CreateTaskRequest request, 
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await createTaskHandler.Handle(request, cancellationToken);
        
        return CreatedAtAction(
            nameof(GetTask), 
            new { taskId = response.TaskId }, 
            response);
    }
    
    /// <summary>
    /// Gets the task result/status by its ID.
    /// </summary>
    /// <param name="taskId">The task ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task status and results if available</returns>
    [HttpGet("{taskId:guid}")]
    [ProducesResponseType(typeof(PendingTaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RunningTaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CompletedTaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(FailedTaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CancelledTaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTask(
        Guid taskId, 
        CancellationToken cancellationToken)
    {
        var request = new GetTaskRequest(taskId);
        var response = await getTaskHandler.Handle(request, cancellationToken);

        if (response == null)
        {
            return NotFound();
        }

        return Ok(response);
    }
    
    /// <summary>
    /// Cancels a pending or running task.
    /// </summary>
    /// <param name="taskId">The task ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Cancelled task response</returns>
    [HttpPatch("{taskId:guid}/cancel")]
    [ProducesResponseType(typeof(CancelTaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CancelTask(
        Guid taskId,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new CancelTaskRequest(taskId);
            var response = await cancelTaskHandler.Handle(request, cancellationToken);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
