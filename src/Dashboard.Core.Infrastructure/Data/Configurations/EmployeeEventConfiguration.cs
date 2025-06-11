using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dashboard.Infrastructure.Data.Configurations;
public class EmployeeEventConfiguration : IEntityTypeConfiguration<EmployeeEvent>
{
    public void Configure(EntityTypeBuilder<EmployeeEvent> builder)
    {
        builder.ToTable("EmployeeEvents");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.EmployeeName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.EventType)
            .HasConversion<int>();

        builder.Property(e => e.ConfidenceScore)
            .HasColumnType("decimal(5,2)");

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.CreatedBy)
            .IsRequired();

        // Indexes for performance
        builder.HasIndex(e => new { e.EmployeeId, e.EventDateTime })
            .HasDatabaseName("IX_EmployeeEvents_EmployeeId_DateTime");

        builder.HasIndex(e => new { e.LocationId, e.EventDateTime })
            .HasDatabaseName("IX_EmployeeEvents_LocationId_DateTime");

        builder.HasIndex(e => new { e.CameraId, e.EventDateTime })
            .HasDatabaseName("IX_EmployeeEvents_CameraId_DateTime");

        builder.HasIndex(e => e.IsDeleted)
            .HasDatabaseName("IX_EmployeeEvents_IsDeleted");

        // Relationships
        builder.HasOne(e => e.Camera)
            .WithMany(c => c.EmployeeEvents)
            .HasForeignKey(e => e.CameraId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Location)
            .WithMany(l => l.EmployeeEvents)
            .HasForeignKey(e => e.LocationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
