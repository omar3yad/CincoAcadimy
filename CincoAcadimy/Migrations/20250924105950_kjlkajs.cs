using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CincoAcadimy.Migrations
{
    /// <inheritdoc />
    public partial class kjlkajs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubmissionLink",
                table: "StudentAssessments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubmissionLink",
                table: "StudentAssessments");
        }
    }
}
