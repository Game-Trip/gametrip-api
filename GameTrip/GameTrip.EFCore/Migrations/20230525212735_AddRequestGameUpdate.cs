using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameTrip.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestGameUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RequestGameUpdateIdRequestGameUpdate",
                table: "Picture",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RequestGameUpdateIdRequestGameUpdate",
                table: "Location",
                type: "uniqueidentifier",
                nullable: true)
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "LocationHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "RequestGameUpdate",
                columns: table => new
                {
                    IdRequestGameUpdate = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Editor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseDate = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestGameUpdate", x => x.IdRequestGameUpdate);
                    table.ForeignKey(
                        name: "FK_RequestGameUpdate_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "IdGame",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Picture_RequestGameUpdateIdRequestGameUpdate",
                table: "Picture",
                column: "RequestGameUpdateIdRequestGameUpdate");

            migrationBuilder.CreateIndex(
                name: "IX_Location_RequestGameUpdateIdRequestGameUpdate",
                table: "Location",
                column: "RequestGameUpdateIdRequestGameUpdate");

            migrationBuilder.CreateIndex(
                name: "IX_RequestGameUpdate_GameId",
                table: "RequestGameUpdate",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_RequestGameUpdate_RequestGameUpdateIdRequestGameUpdate",
                table: "Location",
                column: "RequestGameUpdateIdRequestGameUpdate",
                principalTable: "RequestGameUpdate",
                principalColumn: "IdRequestGameUpdate");

            migrationBuilder.AddForeignKey(
                name: "FK_Picture_RequestGameUpdate_RequestGameUpdateIdRequestGameUpdate",
                table: "Picture",
                column: "RequestGameUpdateIdRequestGameUpdate",
                principalTable: "RequestGameUpdate",
                principalColumn: "IdRequestGameUpdate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_RequestGameUpdate_RequestGameUpdateIdRequestGameUpdate",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_Picture_RequestGameUpdate_RequestGameUpdateIdRequestGameUpdate",
                table: "Picture");

            migrationBuilder.DropTable(
                name: "RequestGameUpdate");

            migrationBuilder.DropIndex(
                name: "IX_Picture_RequestGameUpdateIdRequestGameUpdate",
                table: "Picture");

            migrationBuilder.DropIndex(
                name: "IX_Location_RequestGameUpdateIdRequestGameUpdate",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "RequestGameUpdateIdRequestGameUpdate",
                table: "Picture");

            migrationBuilder.DropColumn(
                name: "RequestGameUpdateIdRequestGameUpdate",
                table: "Location")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "LocationHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
        }
    }
}
