using HttpTaskService.Application.Services;
using HttpTaskService.Application.Tasks;
using HttpTaskService.Application.Tasks.CreateTask;
using HttpTaskService.Infrastructure.BackgroundServices;
using HttpTaskService.Infrastructure.Repositories;
using HttpTaskService.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HttpTaskService.Infrastructure;

public static class Injector
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ApplicationDbContext>();
        services.AddScoped<ITasksRepository, TasksRepository>();
        services.AddScoped<CreateTaskHandler>();
        services.AddScoped<IHttpTaskExecutor, HttpTaskExecutor>();
        services.AddHostedService<HttpTaskBackgroundService>();
        services.AddHttpClient();
        
        return services;
    }
}