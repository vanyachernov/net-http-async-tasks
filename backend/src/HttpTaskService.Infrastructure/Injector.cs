using HttpTaskService.Application.Services;
using HttpTaskService.Application.Tasks;
using HttpTaskService.Application.Tasks.CancelTask;
using HttpTaskService.Application.Tasks.CreateTask;
using HttpTaskService.Application.Tasks.GetTask;
using HttpTaskService.Infrastructure.BackgroundServices;
using HttpTaskService.Infrastructure.Repositories;
using HttpTaskService.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HttpTaskService.Infrastructure;

public static class Injector
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Database
        services.AddScoped<ApplicationDbContext>();
        
        // Repositories
        services.AddScoped<ITasksRepository, TasksRepository>();
        
        // Handlers
        services.AddScoped<CreateTaskHandler>();
        services.AddScoped<GetTaskHandler>();
        services.AddScoped<CancelTaskHandler>();
        
        // HTTP Client
        services.AddHttpClient();
        
        // Services
        services.AddScoped<IHttpTaskExecutor, HttpTaskExecutor>();
        
        // Background Services
        services.AddHostedService<HttpTaskBackgroundService>();
        
        return services;
    }
}