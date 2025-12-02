using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HttpTaskService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCancelledAndRetryFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "cancelled_at",
                table: "http_tasks",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_retry_at",
                table: "http_tasks",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "retry_count",
                table: "http_tasks",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cancelled_at",
                table: "http_tasks");

            migrationBuilder.DropColumn(
                name: "last_retry_at",
                table: "http_tasks");

            migrationBuilder.DropColumn(
                name: "retry_count",
                table: "http_tasks");
        }
    }
}
