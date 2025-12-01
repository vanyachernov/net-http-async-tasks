using HttpTaskService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HttpTaskService.Infrastructure.Configurations;

public class HttpTaskConfiguration : IEntityTypeConfiguration<HttpTask>
{
    public void Configure(EntityTypeBuilder<HttpTask> builder)
    {
        builder.ToTable("HttpTasks");
        
        builder.HasKey(t => t.Id);
        
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
            .HasMaxLength(2000);
        
        builder.HasOne(t => t.Result)
            .WithOne(r => r.HttpTask)
            .HasForeignKey<HttpTaskResult>(r => r.HttpTaskId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(t => t.Status)
            .HasDatabaseName("IX_HttpTasks_Status");
        
        builder.HasIndex(t => t.CreatedAt)
            .HasDatabaseName("IX_HttpTasks_CreatedAt");
    }
}
