using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameTrip.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestLocationUpdateFixDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "RequestLocationUpdate",
                type: "decimal(18,12)",
                precision: 18,
                scale: 12,
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float(18)",
                oldPrecision: 18,
                oldScale: 12,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "RequestLocationUpdate",
                type: "float(18)",
                precision: 18,
                scale: 12,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,12)",
                oldPrecision: 18,
                oldScale: 12,
                oldNullable: true);
        }
    }
}
