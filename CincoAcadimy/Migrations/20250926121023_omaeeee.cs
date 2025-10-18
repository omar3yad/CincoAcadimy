using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CincoAcadimy.Migrations
{
    /// <inheritdoc />
    public partial class omaeeee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "StudentCourses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnrolled",
                table: "StudentCourses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Progress",
                table: "StudentCourses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "StudentCourses");

            migrationBuilder.DropColumn(
                name: "IsEnrolled",
                table: "StudentCourses");

            migrationBuilder.DropColumn(
                name: "Progress",
                table: "StudentCourses");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Courses");
        }
    }
}
