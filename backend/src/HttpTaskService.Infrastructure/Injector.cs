using HttpTaskService.Application.Tasks;
using HttpTaskService.Application.Tasks.CreateTask;
using HttpTaskService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HttpTaskService.Infrastructure;

public static class Injector
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ApplicationDbContext>();
        services.AddScoped<ITasksRepository, TasksRepository>();
        services.AddScoped<CreateTaskHandler>();
        
        return services;
    }
}