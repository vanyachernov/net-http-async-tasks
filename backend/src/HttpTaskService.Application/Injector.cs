using HttpTaskService.Application.Services;
using HttpTaskService.Application.Tasks;
using HttpTaskService.Application.Tasks.CreateTask;
using HttpTaskService.Application.Tasks.GetTask;
using Microsoft.Extensions.DependencyInjection;

namespace HttpTaskService.Application;

public static class Injector
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateTaskHandler>();
        services.AddScoped<GetTaskHandler>();
        
        return services;
    }
}