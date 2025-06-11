using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dashboard.Infrastructure.Data.Configurations;
public class DashboardLocationConfiguration : IEntityTypeConfiguration<DashboardLocation>
{
    public void Configure(EntityTypeBuilder<DashboardLocation> builder)
    {
        builder.ToTable("DashboardLocations");
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(l => l.Description)
            .HasMaxLength(500);

        builder.Property(l => l.Address)
            .HasMaxLength(500);

        builder.Property(l => l.WorkingHoursStart)
            .IsRequired();

        builder.Property(l => l.WorkingHoursEnd)
            .IsRequired();

        builder.Property(l => l.CreatedAt)
            .IsRequired();

        builder.Property(l => l.CreatedBy)
            .IsRequired();

        // Indexes
        builder.HasIndex(l => l.Name)
            .IsUnique()
            .HasDatabaseName("IX_DashboardLocations_Name");

        builder.HasIndex(l => l.IsActive)
            .HasDatabaseName("IX_DashboardLocations_IsActive");

        builder.HasIndex(l => l.IsDeleted)
            .HasDatabaseName("IX_DashboardLocations_IsDeleted");
    }
}
