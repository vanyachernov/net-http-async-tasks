using HttpTaskService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HttpTaskService.Infrastructure.Configurations;

public class HttpTaskConfiguration : IEntityTypeConfiguration<HttpTask>
{
    public void Configure(EntityTypeBuilder<HttpTask> builder)
    {
        builder.ToTable("http_tasks");
        
        builder.HasKey(t => t.Id);
        
        builder
            .Property(t => t.Url)
            .IsRequired()
            .HasMaxLength(2048);
        
        builder
            .Property(t => t.Status)
            .IsRequired();
        
        builder
            .Property(t => t.CreatedAt)
            .IsRequired();
        
        builder
            .Property(t => t.StartedAt)
            .IsRequired(false);
        
        builder
            .Property(t => t.CompletedAt)
            .IsRequired(false);
        
        builder
            .Property(t => t.StatusCode)
            .IsRequired(false);
        
        builder
            .Property(t => t.ContentLength)
            .IsRequired(false);
        
        builder
            .Property(t => t.DurationMs)
            .IsRequired(false);
        
        builder
            .HasIndex(t => t.Status)
            .HasDatabaseName("IDX_HttpTasks_Status");
        
        builder
            .HasIndex(t => t.CreatedAt)
            .HasDatabaseName("IDX_HttpTasks_CreatedAt");
    }
}
