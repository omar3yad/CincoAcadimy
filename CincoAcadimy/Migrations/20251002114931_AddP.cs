using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CincoAcadimy.Migrations
{
    /// <inheritdoc />
    public partial class AddP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Payments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Courses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Courses");
        }
    }
}
