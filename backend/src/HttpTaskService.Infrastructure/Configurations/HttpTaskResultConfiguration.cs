using HttpTaskService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HttpTaskService.Infrastructure.Configurations;

public class HttpTaskResultConfiguration : IEntityTypeConfiguration<HttpTaskResult>
{
    public void Configure(EntityTypeBuilder<HttpTaskResult> builder)
    {
        builder.ToTable("HttpTaskResults");
        
        builder.HasKey(r => r.Id);
        
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

        builder.HasIndex(r => r.HttpTaskId)
            .IsUnique()
            .HasDatabaseName("IX_HttpTaskResults_HttpTaskId");
    }
}
