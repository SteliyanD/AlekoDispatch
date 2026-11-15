using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlekoDispatch.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCompletedDateToWorkSite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedDate",
                table: "WorkSites",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedDate",
                table: "WorkSites");
        }
    }
}
