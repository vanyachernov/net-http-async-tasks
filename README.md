# Http Task Service

A robust asynchronous HTTP task processing service built with .NET 9. This service allows users to schedule HTTP requests to be executed in the background, track their status, and manage their lifecycle.

## 🚀 Technologies

- **.NET 9** (Web API, Worker Service)
- **Entity Framework Core** (PostgreSQL)
- **xUnit, Moq, FluentAssertions** (Testing)
- **Microsoft.Extensions.Logging** (Structured Logging)

## ✨ Features

- **Asynchronous Processing**: Tasks are queued and processed in the background without blocking the API.
- **Status Tracking**: Real-time tracking of task lifecycle: `Pending` -> `Running` -> `Completed` / `Failed` / `Cancelled`.
- **Retry Logic**: Automatic retries for failed tasks with exponential backoff (default: 3 retries with 5s, 10s, 20s delays).
- **Cancellation**: Ability to cancel pending or running tasks.
- **Robust Error Handling**: Comprehensive logging and exception handling to prevent data loss.

## 🛠️ Getting Started

### Prerequisites

- .NET 9 SDK
- PostgreSQL Database

### Configuration

Update `appsettings.json` in `src/HttpTaskService.Api` with your database connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=HttpTaskDb;Username=postgres;Password=yourpassword"
}
```

### Running the Application

1. **Apply Migrations**:
   ```bash
   dotnet ef database update --project src/HttpTaskService.Infrastructure --startup-project src/HttpTaskService.Api
   ```

2. **Start the API**:
   ```bash
   dotnet run --project src/HttpTaskService.Api
   ```
   The API will be available at `http://localhost:5216`.

## 📚 API Documentation

### 1. Create Task
Schedule a new HTTP task.

- **Endpoint**: `POST /api/tasks`
- **Body**:
  ```json
  {
    "url": "https://example.com"
  }
  ```
- **Response**: `201 Created`
  ```json
  {
    "taskId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "status": "pending"
  }
  ```

### 2. Get Task Status
Retrieve the current status and result of a task.

- **Endpoint**: `GET /api/tasks/{id}`
- **Response**: `200 OK`
  ```json
  {
    "taskId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "status": "completed",
    "result": {
      "url": "https://example.com",
      "statusCode": 200,
      "length": 1234,
      "durationMs": 150,
      "completedAt": "2023-10-01T12:00:00Z"
    }
  }
  ```

### 3. Cancel Task
Cancel a pending or running task.

- **Endpoint**: `PATCH /api/tasks/{id}/cancel`
- **Response**: `200 OK`
  ```json
  {
    "taskId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "status": "cancelled"
  }
  ```

## 🧪 Testing

The project includes a comprehensive unit test suite covering all handlers and business logic.

Run tests using the .NET CLI:
```bash
dotnet test src/HttpTaskService.Tests/HttpTaskService.Tests.csproj
```
