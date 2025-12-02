using HttpTaskService.Application.Tasks.CreateTask;
using Microsoft.AspNetCore.Mvc;

namespace HttpTaskService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController(CreateTaskHandler createTaskHandler) : ControllerBase
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
    /// Gets the task by ID (will be implemented later).
    /// </summary>
    [HttpGet("{taskId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetTask(Guid taskId)
    {
        return NotFound();
    }
}
