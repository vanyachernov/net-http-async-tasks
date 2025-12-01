namespace HttpTaskService.Domain.Entities;

/// <summary>
/// Represents the result of an executed HTTP request task.
/// </summary>
public class HttpTaskResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the result.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the foreign key to the associated HTTP task.
    /// </summary>
    public Guid HttpTaskId { get; set; }
    
    /// <summary>
    /// Gets or sets the URL that was requested.
    /// </summary>
    public string Url { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the HTTP response status code.
    /// </summary>
    public int StatusCode { get; set; }
    
    /// <summary>
    /// Gets or sets the length of the response content in bytes.
    /// </summary>
    public long ContentLength { get; set; }
    
    /// <summary>
    /// Gets or sets the duration of the request execution in milliseconds.
    /// </summary>
    public long DurationMs { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the request completed.
    /// </summary>
    public DateTime CompletedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the associated HTTP task.
    /// </summary>
    public HttpTask HttpTask { get; set; } = null!;
}
