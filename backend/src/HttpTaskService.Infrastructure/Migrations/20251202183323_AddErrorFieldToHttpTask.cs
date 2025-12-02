using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HttpTaskService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddErrorFieldToHttpTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "error",
                table: "http_tasks",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "error",
                table: "http_tasks");
        }
    }
}
