using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameTrip.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class SetupPicture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "Picture");

            migrationBuilder.RenameColumn(
                name: "vote",
                table: "LikedGame",
                newName: "Vote");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Picture",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "Picture",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "Picture");

            migrationBuilder.RenameColumn(
                name: "Vote",
                table: "LikedGame",
                newName: "vote");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Picture",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Picture",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}
