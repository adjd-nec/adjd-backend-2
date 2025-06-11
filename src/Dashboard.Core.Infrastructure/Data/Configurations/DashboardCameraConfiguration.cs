using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dashboard.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dashboard.Infrastructure.Data.Configurations;
public class DashboardCameraConfiguration : IEntityTypeConfiguration<DashboardCamera>
{
    public void Configure(EntityTypeBuilder<DashboardCamera> builder)
    {
        builder.ToTable("DashboardCameras");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(c => c.CameraType)
            .HasConversion<int>();

        builder.Property(c => c.Status)
            .HasConversion<int>();

        builder.Property(c => c.IPAddress)
            .HasMaxLength(50);

        builder.Property(c => c.PositionX)
            .HasColumnType("decimal(10,2)");

        builder.Property(c => c.PositionY)
            .HasColumnType("decimal(10,2)");

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.CreatedBy)
            .IsRequired();

        // Indexes
        builder.HasIndex(c => c.NeoFaceCameraId)
            .IsUnique()
            .HasDatabaseName("IX_DashboardCameras_NeoFaceCameraId");

        builder.HasIndex(c => new { c.LocationId, c.IsActive })
            .HasDatabaseName("IX_DashboardCameras_LocationId_IsActive");

        builder.HasIndex(c => c.IsDeleted)
            .HasDatabaseName("IX_DashboardCameras_IsDeleted");

        // Relationships
        builder.HasOne(c => c.Location)
            .WithMany(l => l.Cameras)
            .HasForeignKey(c => c.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.FloorPlan)
            .WithMany(f => f.Cameras)
            .HasForeignKey(c => c.FloorPlanId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
