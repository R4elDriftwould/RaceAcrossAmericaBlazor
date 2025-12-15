using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceAcrossAmerica.Migrations
{
    /// <inheritdoc />
    public partial class AddRaceParticipantsList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RaceParticipants_RaceId",
                table: "RaceParticipants",
                column: "RaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_RaceParticipants_Races_RaceId",
                table: "RaceParticipants",
                column: "RaceId",
                principalTable: "Races",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RaceParticipants_Races_RaceId",
                table: "RaceParticipants");

            migrationBuilder.DropIndex(
                name: "IX_RaceParticipants_RaceId",
                table: "RaceParticipants");
        }
    }
}
