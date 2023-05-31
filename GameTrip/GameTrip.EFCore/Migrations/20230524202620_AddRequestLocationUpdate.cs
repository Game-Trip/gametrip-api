using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameTrip.EFCore.Migrations;

/// <inheritdoc />
public partial class AddRequestLocationUpdate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "RequestLocationUpdateIdRequestLocationUpdate",
            table: "Picture",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "RequestLocationUpdateIdRequestLocationUpdate",
            table: "Game",
            type: "uniqueidentifier",
            nullable: true)
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "GameHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.CreateTable(
            name: "RequestLocationUpdate",
            columns: table => new
            {
                IdRequestLocationUpdate = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Latitude = table.Column<decimal>(type: "decimal(18,12)", precision: 18, scale: 12, nullable: true),
                Longitude = table.Column<double>(type: "decimal(18,12)", precision: 18, scale: 12, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RequestLocationUpdate", x => x.IdRequestLocationUpdate);
                table.ForeignKey(
                    name: "FK_RequestLocationUpdate_Location_LocationId",
                    column: x => x.LocationId,
                    principalTable: "Location",
                    principalColumn: "IdLocation",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Picture_RequestLocationUpdateIdRequestLocationUpdate",
            table: "Picture",
            column: "RequestLocationUpdateIdRequestLocationUpdate");

        migrationBuilder.CreateIndex(
            name: "IX_Game_RequestLocationUpdateIdRequestLocationUpdate",
            table: "Game",
            column: "RequestLocationUpdateIdRequestLocationUpdate");

        migrationBuilder.CreateIndex(
            name: "IX_RequestLocationUpdate_LocationId",
            table: "RequestLocationUpdate",
            column: "LocationId");

        migrationBuilder.AddForeignKey(
            name: "FK_Game_RequestLocationUpdate_RequestLocationUpdateIdRequestLocationUpdate",
            table: "Game",
            column: "RequestLocationUpdateIdRequestLocationUpdate",
            principalTable: "RequestLocationUpdate",
            principalColumn: "IdRequestLocationUpdate");

        migrationBuilder.AddForeignKey(
            name: "FK_Picture_RequestLocationUpdate_RequestLocationUpdateIdRequestLocationUpdate",
            table: "Picture",
            column: "RequestLocationUpdateIdRequestLocationUpdate",
            principalTable: "RequestLocationUpdate",
            principalColumn: "IdRequestLocationUpdate");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Game_RequestLocationUpdate_RequestLocationUpdateIdRequestLocationUpdate",
            table: "Game");

        migrationBuilder.DropForeignKey(
            name: "FK_Picture_RequestLocationUpdate_RequestLocationUpdateIdRequestLocationUpdate",
            table: "Picture");

        migrationBuilder.DropTable(
            name: "RequestLocationUpdate");

        migrationBuilder.DropIndex(
            name: "IX_Picture_RequestLocationUpdateIdRequestLocationUpdate",
            table: "Picture");

        migrationBuilder.DropIndex(
            name: "IX_Game_RequestLocationUpdateIdRequestLocationUpdate",
            table: "Game");

        migrationBuilder.DropColumn(
            name: "RequestLocationUpdateIdRequestLocationUpdate",
            table: "Picture");

        migrationBuilder.DropColumn(
            name: "RequestLocationUpdateIdRequestLocationUpdate",
            table: "Game")
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "GameHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
    }
}
