using FluentAssertions;
using HttpTaskService.Application.Tasks;
using HttpTaskService.Application.Tasks.GetTask;
using HttpTaskService.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using TaskStatus = HttpTaskService.Domain.Shared.TaskStatus;

namespace HttpTaskService.Tests.Application.Tasks.GetTask;

public class GetTaskHandlerTests
{
    private readonly Mock<ITasksRepository> _tasksRepositoryMock;
    private readonly Mock<ILogger<GetTaskHandler>> _loggerMock;
    private readonly GetTaskHandler _handler;

    public GetTaskHandlerTests()
    {
        _tasksRepositoryMock = new Mock<ITasksRepository>();
        _loggerMock = new Mock<ILogger<GetTaskHandler>>();
        _handler = new GetTaskHandler(_tasksRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnPending_WhenTaskIsPending()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var task = new HttpTask { Id = taskId, Status = TaskStatus.Pending };
        _tasksRepositoryMock.Setup(x => x.GetTaskByIdAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);

        // Act
        var result = await _handler.Handle(new GetTaskRequest(taskId), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PendingTaskResponse>();
        result!.Status.Should().Be("pending");
    }

    [Fact]
    public async Task Handle_ShouldReturnRunning_WhenTaskIsRunning()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var task = new HttpTask { Id = taskId, Status = TaskStatus.Running };
        _tasksRepositoryMock.Setup(x => x.GetTaskByIdAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);

        // Act
        var result = await _handler.Handle(new GetTaskRequest(taskId), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<RunningTaskResponse>();
        result!.Status.Should().Be("running");
    }

    [Fact]
    public async Task Handle_ShouldReturnCompleted_WhenTaskIsCompleted()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var task = new HttpTask 
        { 
            Id = taskId, 
            Status = TaskStatus.Completed,
            Url = "https://example.com",
            StatusCode = 200,
            ContentLength = 100,
            DurationMs = 500,
            CompletedAt = DateTime.UtcNow
        };
        _tasksRepositoryMock.Setup(x => x.GetTaskByIdAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);

        // Act
        var result = await _handler.Handle(new GetTaskRequest(taskId), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CompletedTaskResponse>();
        result!.Status.Should().Be("completed");
        
        var completedResponse = (CompletedTaskResponse)result;
        completedResponse.ResultDto.Url.Should().Be(task.Url);
        completedResponse.ResultDto.StatusCode.Should().Be(task.StatusCode);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailed_WhenTaskIsFailed()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var task = new HttpTask 
        { 
            Id = taskId, 
            Status = TaskStatus.Failed,
            Error = "Some error"
        };
        _tasksRepositoryMock.Setup(x => x.GetTaskByIdAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);

        // Act
        var result = await _handler.Handle(new GetTaskRequest(taskId), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<FailedTaskResponse>();
        result!.Status.Should().Be("failed");
        ((FailedTaskResponse)result).Error.Should().Be(task.Error);
    }

    [Fact]
    public async Task Handle_ShouldReturnCancelled_WhenTaskIsCancelled()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var task = new HttpTask 
        { 
            Id = taskId, 
            Status = TaskStatus.Cancelled,
            CancelledAt = DateTime.UtcNow
        };
        _tasksRepositoryMock.Setup(x => x.GetTaskByIdAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);

        // Act
        var result = await _handler.Handle(new GetTaskRequest(taskId), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CancelledTaskResponse>();
        result!.Status.Should().Be("cancelled");
        ((CancelledTaskResponse)result).CancelledAt.Should().Be(task.CancelledAt.Value);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenTaskNotFound()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        _tasksRepositoryMock.Setup(x => x.GetTaskByIdAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((HttpTask?)null);

        // Act
        var result = await _handler.Handle(new GetTaskRequest(taskId), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}
