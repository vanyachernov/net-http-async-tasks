using HttpTaskService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HttpTaskService.Infrastructure;

/// <summary>
/// Entity Framework database context for the HTTP Task Service.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    /// <summary>
    /// Gets or sets the DbSet of HTTP tasks.
    /// </summary>
    public DbSet<HttpTask> HttpTasks { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the DbSet of HTTP task results.
    /// </summary>
    public DbSet<HttpTaskResult> HttpTaskResults { get; set; } = null!;
    
    /// <summary>
    /// Configures the database model using Fluent API.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Apply all entity configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
