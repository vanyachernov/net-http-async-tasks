namespace HttpTaskService.Domain.Shared;

/// <summary>
/// Represents the execution status of an HTTP task.
/// </summary>
public enum TaskStatus
{
    /// <summary>
    /// Task has been created and is waiting to be executed.
    /// </summary>
    Pending = 0,
    
    /// <summary>
    /// Task is currently being executed.
    /// </summary>
    Running = 1,
    
    /// <summary>
    /// Task has completed successfully.
    /// </summary>
    Completed = 2,
    
    /// <summary>
    /// Task execution has failed.
    /// </summary>
    Failed = 3,
    
    /// <summary>
    /// Task has been cancelled by user.
    /// </summary>
    Cancelled = 4
}
