namespace HttpTaskService.Application.DTOs;

public class HttpTaskResultDto
{
    public string Url { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public long ContentLength { get; set; }
    public long DurationMs { get; set; }
}