using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameTrip.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class NoActionForeingkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestGameUpdate_Game_GameId",
                table: "RequestGameUpdate");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestLocationUpdate_Location_LocationId",
                table: "RequestLocationUpdate");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestGameUpdate_Game_GameId",
                table: "RequestGameUpdate",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "IdGame");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestLocationUpdate_Location_LocationId",
                table: "RequestLocationUpdate",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "IdLocation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestGameUpdate_Game_GameId",
                table: "RequestGameUpdate");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestLocationUpdate_Location_LocationId",
                table: "RequestLocationUpdate");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestGameUpdate_Game_GameId",
                table: "RequestGameUpdate",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "IdGame",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestLocationUpdate_Location_LocationId",
                table: "RequestLocationUpdate",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "IdLocation",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
