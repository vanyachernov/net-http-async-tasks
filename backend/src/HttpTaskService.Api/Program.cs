using HttpTaskService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddOpenApi();

    builder.Services.AddInfrastructure();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }
    
    app.MapGet("/health", () => Results.Ok("My service is working"));

    app.Run();
}