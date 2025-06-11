using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dashboard.Infrastructure.Data.Configurations;
public class EmployeeAlertConfiguration : IEntityTypeConfiguration<EmployeeAlert>
{
    public void Configure(EntityTypeBuilder<EmployeeAlert> builder)
    {
        builder.ToTable("EmployeeAlerts");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.EmployeeName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(a => a.AlertType)
            .HasConversion<int>();

        builder.Property(a => a.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(a => a.CreatedAt)
            .IsRequired();

        builder.Property(a => a.CreatedBy)
            .IsRequired();

        // Indexes for performance
        builder.HasIndex(a => new { a.EmployeeId, a.AlertDateTime })
            .HasDatabaseName("IX_EmployeeAlerts_EmployeeId_DateTime");

        builder.HasIndex(a => new { a.LocationId, a.IsResolved })
            .HasDatabaseName("IX_EmployeeAlerts_LocationId_IsResolved");

        builder.HasIndex(a => a.IsDeleted)
            .HasDatabaseName("IX_EmployeeAlerts_IsDeleted");

        // Relationships
        builder.HasOne(a => a.Location)
            .WithMany(l => l.EmployeeAlerts)
            .HasForeignKey(a => a.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.RelatedEvent)
            .WithMany()
            .HasForeignKey(a => a.RelatedEventId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
