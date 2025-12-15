using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceAcrossAmerica.Migrations
{
    /// <inheritdoc />
    public partial class FixParticipantStudentLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "RaceParticipants");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "RaceParticipants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RaceParticipants_StudentId",
                table: "RaceParticipants",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_RaceParticipants_Students_StudentId",
                table: "RaceParticipants",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RaceParticipants_Students_StudentId",
                table: "RaceParticipants");

            migrationBuilder.DropIndex(
                name: "IX_RaceParticipants_StudentId",
                table: "RaceParticipants");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "RaceParticipants");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RaceParticipants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
