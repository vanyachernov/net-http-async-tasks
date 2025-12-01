using HttpTaskService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HttpTaskService.Infrastructure.Configurations;

/// <summary>
/// Entity Framework configuration for the HttpTask entity.
/// </summary>
public class HttpTaskConfiguration : IEntityTypeConfiguration<HttpTask>
{
    /// <summary>
    /// Configures the HttpTask entity.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<HttpTask> builder)
    {
        // Table name
        builder.ToTable("HttpTasks");
        
        // Primary key
        builder.HasKey(t => t.Id);
        
        // Properties
        builder.Property(t => t.Url)
            .IsRequired()
            .HasMaxLength(2048);
        
        builder.Property(t => t.Status)
            .IsRequired();
        
        builder.Property(t => t.CreatedAt)
            .IsRequired();
        
        builder.Property(t => t.StartedAt)
            .IsRequired(false);
        
        builder.Property(t => t.CompletedAt)
            .IsRequired(false);
        
        builder.Property(t => t.ErrorMessage)
            .IsRequired(false)
            .HasMaxLength(4000);
        
        // Relationship with HttpTaskResult (one-to-one, optional)
        builder.HasOne(t => t.Result)
            .WithOne(r => r.HttpTask)
            .HasForeignKey<HttpTaskResult>(r => r.HttpTaskId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Indexes for better query performance
        builder.HasIndex(t => t.Status)
            .HasDatabaseName("IX_HttpTasks_Status");
        
        builder.HasIndex(t => t.CreatedAt)
            .HasDatabaseName("IX_HttpTasks_CreatedAt");
    }
}
