using HttpTaskService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HttpTaskService.Infrastructure.Configurations;

/// <summary>
/// Entity Framework configuration for the HttpTaskResult entity.
/// </summary>
public class HttpTaskResultConfiguration : IEntityTypeConfiguration<HttpTaskResult>
{
    /// <summary>
    /// Configures the HttpTaskResult entity.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<HttpTaskResult> builder)
    {
        // Table name
        builder.ToTable("HttpTaskResults");
        
        // Primary key
        builder.HasKey(r => r.Id);
        
        // Properties
        builder.Property(r => r.HttpTaskId)
            .IsRequired();
        
        builder.Property(r => r.Url)
            .IsRequired()
            .HasMaxLength(2048);
        
        builder.Property(r => r.StatusCode)
            .IsRequired();
        
        builder.Property(r => r.ContentLength)
            .IsRequired();
        
        builder.Property(r => r.DurationMs)
            .IsRequired();
        
        builder.Property(r => r.CompletedAt)
            .IsRequired();
        
        // Index on HttpTaskId for faster lookups
        builder.HasIndex(r => r.HttpTaskId)
            .IsUnique()
            .HasDatabaseName("IX_HttpTaskResults_HttpTaskId");
    }
}
