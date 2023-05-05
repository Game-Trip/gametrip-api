using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameTrip.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class updateGameEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RealaseDate",
                table: "Game");

            migrationBuilder.AddColumn<long>(
                name: "ReleaseDate",
                table: "Game",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "Game");

            migrationBuilder.AddColumn<long>(
                name: "RealaseDate",
                table: "Game",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
