using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace FSH.Starter.WebApi.Migrations.MSSQL.AdjdDashboard
{
    /// <inheritdoc />
    public partial class AddAdjdDashboardSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create adjd schema
            migrationBuilder.EnsureSchema(
                name: "adjd");

            // Create Locations table
            migrationBuilder.CreateTable(
                name: "Locations",
                schema: "adjd",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Building = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Floor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Zone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FloorPlanImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    FloorPlanFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FloorPlanContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            // Create WatchLists table
            migrationBuilder.CreateTable(
                name: "WatchLists",
                schema: "adjd",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NeoFaceWatchListId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkingHoursStart = table.Column<TimeSpan>(type: "time", nullable: false),
                    WorkingHoursEnd = table.Column<TimeSpan>(type: "time", nullable: false),
                    WorkingDays = table.Column<int>(type: "int", nullable: false),
                    BreakPeriodsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AwayAlertDurationMinutes = table.Column<int>(type: "int", nullable: false),
                    GracePeriodMinutes = table.Column<int>(type: "int", nullable: false),
                    DoubleCountWindowMinutes = table.Column<int>(type: "int", nullable: false),
                    MissingEventTimeoutMinutes = table.Column<int>(type: "int", nullable: false),
                    AllowMultipleEntries = table.Column<bool>(type: "bit", nullable: false),
                    AllowMultipleExits = table.Column<bool>(type: "bit", nullable: false),
                    BreakTimeFlexibilityMinutes = table.Column<int>(type: "int", nullable: false),
                    ShiftChangeWindowMinutes = table.Column<int>(type: "int", nullable: false),
                    ApplyRulesOnWeekends = table.Column<bool>(type: "bit", nullable: false),
                    ApplyRulesOnHolidays = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchLists", x => x.Id);
                });

            // Create WorkSchedules table
            migrationBuilder.CreateTable(
                name: "WorkSchedules",
                schema: "adjd",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ShiftStart = table.Column<TimeSpan>(type: "time", nullable: true),
                    ShiftEnd = table.Column<TimeSpan>(type: "time", nullable: true),
                    WorkingDays = table.Column<int>(type: "int", nullable: false),
                    IsFlexibleHours = table.Column<bool>(type: "bit", nullable: false),
                    FlexibleStartEarliest = table.Column<TimeSpan>(type: "time", nullable: true),
                    FlexibleStartLatest = table.Column<TimeSpan>(type: "time", nullable: true),
                    CoreHoursRequired = table.Column<int>(type: "int", nullable: false),
                    AllowWorkFromHome = table.Column<bool>(type: "bit", nullable: false),
                    WorkFromHomeDays = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    WatchListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkSchedules_WatchLists_WatchListId",
                        column: x => x.WatchListId,
                        principalSchema: "adjd",
                        principalTable: "WatchLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // Create Cameras table
            migrationBuilder.CreateTable(
                name: "Cameras",
                schema: "adjd",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NeoFaceCameraId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CameraType = table.Column<int>(type: "int", nullable: false),
                    FlowDirection = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    CoverageArea = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FloorPlanX = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FloorPlanY = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsAwayAlertEligible = table.Column<bool>(type: "bit", nullable: false),
                    IsOperational = table.Column<bool>(type: "bit", nullable: false),
                    OperationalHoursStart = table.Column<TimeSpan>(type: "time", nullable: true),
                    OperationalHoursEnd = table.Column<TimeSpan>(type: "time", nullable: true),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cameras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cameras_Locations_LocationId",
                        column: x => x.LocationId,
                        principalSchema: "adjd",
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // Create Employees table
            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "adjd",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NeoFacePersonId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TerminationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PrimaryLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Locations_PrimaryLocationId",
                        column: x => x.PrimaryLocationId,
                        principalSchema: "adjd",
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Employees_WorkSchedules_WorkScheduleId",
                        column: x => x.WorkScheduleId,
                        principalSchema: "adjd",
                        principalTable: "WorkSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            // Create DailyCounters table
            migrationBuilder.CreateTable(
                name: "DailyCounters",
                schema: "adjd",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CounterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TotalEntries = table.Column<int>(type: "int", nullable: false),
                    TotalExits = table.Column<int>(type: "int", nullable: false),
                    CurrentPresent = table.Column<int>(type: "int", nullable: false),
                    MissingEntryAlerts = table.Column<int>(type: "int", nullable: false),
                    MissingExitAlerts = table.Column<int>(type: "int", nullable: false),
                    DoubleEntryAlerts = table.Column<int>(type: "int", nullable: false),
                    DoubleExitAlerts = table.Column<int>(type: "int", nullable: false),
                    AwayAlerts = table.Column<int>(type: "int", nullable: false),
                    PeakPresent = table.Column<int>(type: "int", nullable: false),
                    PeakTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastResetAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsResetComplete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyCounters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyCounters_Locations_LocationId",
                        column: x => x.LocationId,
                        principalSchema: "adjd",
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            // Create EmployeeEvents table
            migrationBuilder.CreateTable(
                name: "EmployeeEvents",
                schema: "adjd",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CameraId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventType = table.Column<int>(type: "int", nullable: false),
                    NeoFaceEventId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfidenceScore = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsProcessed = table.Column<bool>(type: "bit", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsValidated = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeEvents_Cameras_CameraId",
                        column: x => x.CameraId,
                        principalSchema: "adjd",
                        principalTable: "Cameras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeEvents_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "adjd",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // Create Alerts table
            migrationBuilder.CreateTable(
                name: "Alerts",
                schema: "adjd",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AlertType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DismissedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolvedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ResolutionNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CameraId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RelatedEventId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alerts_Cameras_CameraId",
                        column: x => x.CameraId,
                        principalSchema: "adjd",
                        principalTable: "Cameras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Alerts_EmployeeEvents_RelatedEventId",
                        column: x => x.RelatedEventId,
                        principalSchema: "adjd",
                        principalTable: "EmployeeEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Alerts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "adjd",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            // Create CameraConfigurations table
            migrationBuilder.CreateTable(
                name: "CameraConfigurations",
                schema: "adjd",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CameraId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConfigurationJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CameraConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CameraConfigurations_Cameras_CameraId",
                        column: x => x.CameraId,
                        principalSchema: "adjd",
                        principalTable: "Cameras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Create AlertConfigurations table
            migrationBuilder.CreateTable(
                name: "AlertConfigurations",
                schema: "adjd",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AlertType = table.Column<int>(type: "int", nullable: false),
                    EmailNotificationEnabled = table.Column<bool>(type: "bit", nullable: false),
                    DashboardNotificationEnabled = table.Column<bool>(type: "bit", nullable: false),
                    EscalationTimeoutMinutes = table.Column<int>(type: "int", nullable: false),
                    AutoResolveEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AutoResolveAfterMinutes = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertConfigurations", x => x.Id);
                });

            // Create SystemConfigurations table
            migrationBuilder.CreateTable(
                name: "SystemConfigurations",
                schema: "adjd",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConfigKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ConfigValue = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ConfigType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsSystem = table.Column<bool>(type: "bit", nullable: false),
                    IsEncrypted = table.Column<bool>(type: "bit", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemConfigurations", x => x.Id);
                });

            // Create ReportConfigurations table
            migrationBuilder.CreateTable(
                name: "ReportConfigurations",
                schema: "adjd",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReportType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ConfigurationJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportConfigurations", x => x.Id);
                });

            // Create Indexes
            migrationBuilder.CreateIndex(
                name: "IX_Alerts_AlertType",
                schema: "adjd",
                table: "Alerts",
                column: "AlertType");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_CameraId",
                schema: "adjd",
                table: "Alerts",
                column: "CameraId");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_EmployeeId",
                schema: "adjd",
                table: "Alerts",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_RelatedEventId",
                schema: "adjd",
                table: "Alerts",
                column: "RelatedEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_Status_GeneratedAt",
                schema: "adjd",
                table: "Alerts",
                columns: new[] { "Status", "GeneratedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_CameraConfigurations_CameraId",
                schema: "adjd",
                table: "CameraConfigurations",
                column: "CameraId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cameras_LocationId",
                schema: "adjd",
                table: "Cameras",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Cameras_NeoFaceCameraId",
                schema: "adjd",
                table: "Cameras",
                column: "NeoFaceCameraId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DailyCounters_CounterDate",
                schema: "adjd",
                table: "DailyCounters",
                column: "CounterDate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DailyCounters_LocationId",
                schema: "adjd",
                table: "DailyCounters",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEvents_CameraId_EventTime",
                schema: "adjd",
                table: "EmployeeEvents",
                columns: new[] { "CameraId", "EventTime" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEvents_EmployeeId",
                schema: "adjd",
                table: "EmployeeEvents",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEvents_EmployeeId_EventTime",
                schema: "adjd",
                table: "EmployeeEvents",
                columns: new[] { "EmployeeId", "EventTime" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEvents_IsProcessed",
                schema: "adjd",
                table: "EmployeeEvents",
                column: "IsProcessed");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeNumber",
                schema: "adjd",
                table: "Employees",
                column: "EmployeeNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_NeoFacePersonId",
                schema: "adjd",
                table: "Employees",
                column: "NeoFacePersonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PrimaryLocationId",
                schema: "adjd",
                table: "Employees",
                column: "PrimaryLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_WorkScheduleId",
                schema: "adjd",
                table: "Employees",
                column: "WorkScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemConfigurations_ConfigKey",
                schema: "adjd",
                table: "SystemConfigurations",
                column: "ConfigKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkSchedules_WatchListId",
                schema: "adjd",
                table: "WorkSchedules",
                column: "WatchListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertConfigurations",
                schema: "adjd");

            migrationBuilder.DropTable(
                name: "Alerts",
                schema: "adjd");

            migrationBuilder.DropTable(
                name: "CameraConfigurations",
                schema: "adjd");

            migrationBuilder.DropTable(
                name: "DailyCounters",
                schema: "adjd");

            migrationBuilder.DropTable(
                name: "ReportConfigurations",
                schema: "adjd");

            migrationBuilder.DropTable(
                name: "SystemConfigurations",
                schema: "adjd");

            migrationBuilder.DropTable(
                name: "EmployeeEvents",
                schema: "adjd");

            migrationBuilder.DropTable(
                name: "Cameras",
                schema: "adjd");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "adjd");

            migrationBuilder.DropTable(
                name: "Locations",
                schema: "adjd");

            migrationBuilder.DropTable(
                name: "WorkSchedules",
                schema: "adjd");

            migrationBuilder.DropTable(
                name: "WatchLists",
                schema: "adjd");
        }
    }
}
