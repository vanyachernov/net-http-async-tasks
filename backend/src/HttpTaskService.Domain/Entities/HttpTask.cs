using HttpTaskService.Domain.Shared;

namespace HttpTaskService.Domain.Entities;

/// <summary>
/// Represents an HTTP request task to be executed asynchronously.
/// </summary>
public class HttpTask
{
    /// <summary>
    /// Gets or sets the unique identifier of the task.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the target URL for the HTTP request.
    /// </summary>
    public string Url { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the current status of the task.
    /// </summary>
    public TaskStatus Status { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the task was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the task execution started.
    /// </summary>
    public DateTime? StartedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the task completed.
    /// </summary>
    public DateTime? CompletedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the error message if the task failed.
    /// </summary>
    public string? ErrorMessage { get; set; }
    
    /// <summary>
    /// Gets or sets the result of the HTTP request execution.
    /// </summary>
    public HttpTaskResult? Result { get; set; }
}
