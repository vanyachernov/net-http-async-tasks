using FluentAssertions;
using HttpTaskService.Application.Tasks;
using HttpTaskService.Application.Tasks.CancelTask;
using HttpTaskService.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using TaskStatus = HttpTaskService.Domain.Shared.TaskStatus;

namespace HttpTaskService.Tests.Application.Tasks.CancelTask;

public class CancelTaskHandlerTests
{
    private readonly Mock<ITasksRepository> _tasksRepositoryMock;
    private readonly Mock<ILogger<CancelTaskHandler>> _loggerMock;
    private readonly CancelTaskHandler _handler;

    public CancelTaskHandlerTests()
    {
        _tasksRepositoryMock = new Mock<ITasksRepository>();
        _loggerMock = new Mock<ILogger<CancelTaskHandler>>();
        _handler = new CancelTaskHandler(_tasksRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCancelTask_WhenStatusIsPending()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var task = new HttpTask { Id = taskId, Status = TaskStatus.Pending };
        _tasksRepositoryMock.Setup(x => x.GetTaskByIdAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);

        // Act
        var result = await _handler.Handle(new CancelTaskRequest(taskId), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Status.Should().Be("cancelled");
        result.TaskId.Should().Be(taskId);

        _tasksRepositoryMock.Verify(x => x.UpdateTaskAsync(
            It.Is<HttpTask>(t => t.Status == TaskStatus.Cancelled && t.CancelledAt != null),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCancelTask_WhenStatusIsRunning()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var task = new HttpTask { Id = taskId, Status = TaskStatus.Running };
        _tasksRepositoryMock.Setup(x => x.GetTaskByIdAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);

        // Act
        var result = await _handler.Handle(new CancelTaskRequest(taskId), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Status.Should().Be("cancelled");

        _tasksRepositoryMock.Verify(x => x.UpdateTaskAsync(
            It.Is<HttpTask>(t => t.Status == TaskStatus.Cancelled),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenTaskNotFound()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        _tasksRepositoryMock.Setup(x => x.GetTaskByIdAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((HttpTask?)null);

        // Act
        var result = await _handler.Handle(new CancelTaskRequest(taskId), CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _tasksRepositoryMock.Verify(x => x.UpdateTaskAsync(It.IsAny<HttpTask>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [InlineData(TaskStatus.Completed)]
    [InlineData(TaskStatus.Failed)]
    [InlineData(TaskStatus.Cancelled)]
    public async Task Handle_ShouldThrowException_WhenStatusIsInvalid(TaskStatus status)
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var task = new HttpTask { Id = taskId, Status = status };
        _tasksRepositoryMock.Setup(x => x.GetTaskByIdAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);

        // Act
        var act = async () => await _handler.Handle(new CancelTaskRequest(taskId), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Cannot cancel task with status '{status.ToString().ToLower()}'");
            
        _tasksRepositoryMock.Verify(x => x.UpdateTaskAsync(It.IsAny<HttpTask>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
