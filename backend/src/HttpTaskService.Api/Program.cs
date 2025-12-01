var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddOpenApi();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }
    
    app.MapGet("/health", () => Results.Ok("My service is working"));

    // app.UseHttpsRedirection();

    app.Run();
}