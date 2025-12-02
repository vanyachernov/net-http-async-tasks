using FluentAssertions;
using HttpTaskService.Application.Tasks;
using HttpTaskService.Application.Tasks.CreateTask;
using HttpTaskService.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using TaskStatus = HttpTaskService.Domain.Shared.TaskStatus;

namespace HttpTaskService.Tests.Application.Tasks.CreateTask;

public class CreateTaskHandlerTests
{
    private readonly Mock<ITasksRepository> _tasksRepositoryMock;
    private readonly Mock<ILogger<CreateTaskHandler>> _loggerMock;
    private readonly CreateTaskHandler _handler;

    public CreateTaskHandlerTests()
    {
        _tasksRepositoryMock = new Mock<ITasksRepository>();
        _loggerMock = new Mock<ILogger<CreateTaskHandler>>();
        _handler = new CreateTaskHandler(_tasksRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateTask_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateTaskRequest("https://example.com");
        var expectedTask = new HttpTask
        {
            Id = Guid.NewGuid(),
            Url = request.Url,
            Status = TaskStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _tasksRepositoryMock.Setup(x => x.CreateTaskAsync(It.IsAny<HttpTask>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedTask);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TaskId.Should().Be(expectedTask.Id);
        result.Status.Should().Be("pending");

        _tasksRepositoryMock.Verify(x => x.CreateTaskAsync(
            It.Is<HttpTask>(t => t.Url == request.Url && t.Status == TaskStatus.Pending),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenRepositoryFails()
    {
        // Arrange
        var request = new CreateTaskRequest("https://example.com");
        var expectedException = new Exception("Database error");

        _tasksRepositoryMock.Setup(x => x.CreateTaskAsync(It.IsAny<HttpTask>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage(expectedException.Message);

        _tasksRepositoryMock.Verify(x => x.CreateTaskAsync(It.IsAny<HttpTask>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
