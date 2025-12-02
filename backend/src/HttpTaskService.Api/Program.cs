using HttpTaskService.Application;
using HttpTaskService.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddOpenApi();
    builder.Services.AddControllers();
    
    builder.Services
        .AddApplication()
        .AddInfrastructure();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        
        app.MapScalarApiReference(options =>
        {
            options.Title = "Http Tasks Api";
        });
    }

    app.MapControllers();
    app.Run();
}