using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceAcrossAmerica.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentIdToParticipant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RaceParticipants_Students_StudentId",
                table: "RaceParticipants");

            migrationBuilder.RenameColumn(
                name: "CurrentDistanceMiles",
                table: "RaceParticipants",
                newName: "LapsCompleted");

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "RaceParticipants",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RaceParticipants_Students_StudentId",
                table: "RaceParticipants",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RaceParticipants_Students_StudentId",
                table: "RaceParticipants");

            migrationBuilder.RenameColumn(
                name: "LapsCompleted",
                table: "RaceParticipants",
                newName: "CurrentDistanceMiles");

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "RaceParticipants",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_RaceParticipants_Students_StudentId",
                table: "RaceParticipants",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }
    }
}
