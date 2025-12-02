using System.ComponentModel.DataAnnotations;

namespace HttpTaskService.Application.Tasks.CreateTask;

public record CreateTaskRequest(
    [Required(ErrorMessage = "URL is required")]
    [Url(ErrorMessage = "Invalid URL format")]
    string Url);