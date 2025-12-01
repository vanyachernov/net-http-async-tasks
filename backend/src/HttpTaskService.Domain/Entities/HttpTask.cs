using TaskStatus = HttpTaskService.Domain.Shared.TaskStatus;

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
    /// Gets or sets the HTTP response status code.
    /// Populated only when task completes successfully.
    /// </summary>
    public int? StatusCode { get; set; }
    
    /// <summary>
    /// Gets or sets the length of the response content in bytes.
    /// Populated only when task completes successfully.
    /// </summary>
    public long? ContentLength { get; set; }
    
    /// <summary>
    /// Gets or sets the duration of the request execution in milliseconds.
    /// Populated only when task completes successfully.
    /// </summary>
    public long? DurationMs { get; set; }
}
