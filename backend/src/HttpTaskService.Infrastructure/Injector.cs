using Microsoft.Extensions.DependencyInjection;

namespace HttpTaskService.Infrastructure;

public static class Injector
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ApplicationDbContext>();
        
        return services;
    }
}