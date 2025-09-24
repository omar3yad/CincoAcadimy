using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CincoAcadimy.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentAssessment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentAssessment_Assessments_AssessmentId",
                table: "StudentAssessment");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAssessment_Students_StudentId",
                table: "StudentAssessment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentAssessment",
                table: "StudentAssessment");

            migrationBuilder.RenameTable(
                name: "StudentAssessment",
                newName: "StudentAssessments");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAssessment_AssessmentId",
                table: "StudentAssessments",
                newName: "IX_StudentAssessments_AssessmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentAssessments",
                table: "StudentAssessments",
                columns: new[] { "StudentId", "AssessmentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAssessments_Assessments_AssessmentId",
                table: "StudentAssessments",
                column: "AssessmentId",
                principalTable: "Assessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAssessments_Students_StudentId",
                table: "StudentAssessments",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentAssessments_Assessments_AssessmentId",
                table: "StudentAssessments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAssessments_Students_StudentId",
                table: "StudentAssessments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentAssessments",
                table: "StudentAssessments");

            migrationBuilder.RenameTable(
                name: "StudentAssessments",
                newName: "StudentAssessment");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAssessments_AssessmentId",
                table: "StudentAssessment",
                newName: "IX_StudentAssessment_AssessmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentAssessment",
                table: "StudentAssessment",
                columns: new[] { "StudentId", "AssessmentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAssessment_Assessments_AssessmentId",
                table: "StudentAssessment",
                column: "AssessmentId",
                principalTable: "Assessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAssessment_Students_StudentId",
                table: "StudentAssessment",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
