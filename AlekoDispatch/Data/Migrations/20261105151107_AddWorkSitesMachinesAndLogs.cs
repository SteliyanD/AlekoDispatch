using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlekoDispatch.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkSitesMachinesAndLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkSites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TargetCompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkSites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HeavyMachines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    InventoryNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CurrentEngineHours = table.Column<double>(type: "float", nullable: false),
                    IsOperational = table.Column<bool>(type: "bit", nullable: false),
                    CurrentWorkSiteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeavyMachines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HeavyMachines_WorkSites_CurrentWorkSiteId",
                        column: x => x.CurrentWorkSiteId,
                        principalTable: "WorkSites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DailyWorkLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkSiteId = table.Column<int>(type: "int", nullable: false),
                    HeavyMachineId = table.Column<int>(type: "int", nullable: false),
                    HoursWorked = table.Column<double>(type: "float", nullable: false),
                    FuelConsumedLiters = table.Column<double>(type: "float", nullable: false),
                    DebrisHauledTons = table.Column<double>(type: "float", nullable: false),
                    OperatorName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyWorkLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyWorkLogs_HeavyMachines_HeavyMachineId",
                        column: x => x.HeavyMachineId,
                        principalTable: "HeavyMachines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DailyWorkLogs_WorkSites_WorkSiteId",
                        column: x => x.WorkSiteId,
                        principalTable: "WorkSites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyWorkLogs_HeavyMachineId",
                table: "DailyWorkLogs",
                column: "HeavyMachineId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyWorkLogs_WorkSiteId",
                table: "DailyWorkLogs",
                column: "WorkSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_HeavyMachines_CurrentWorkSiteId",
                table: "HeavyMachines",
                column: "CurrentWorkSiteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyWorkLogs");

            migrationBuilder.DropTable(
                name: "HeavyMachines");

            migrationBuilder.DropTable(
                name: "WorkSites");
        }
    }
}
