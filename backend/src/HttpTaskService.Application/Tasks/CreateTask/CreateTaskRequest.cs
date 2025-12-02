using System.ComponentModel.DataAnnotations;

namespace HttpTaskService.Application.Tasks.CreateTask;

/// <summary>
/// Request to create a new task.
/// </summary>
public record CreateTaskRequest(
    [Required(ErrorMessage = "URL is required")]
    [Url(ErrorMessage = "Invalid URL format")]
    string Url);